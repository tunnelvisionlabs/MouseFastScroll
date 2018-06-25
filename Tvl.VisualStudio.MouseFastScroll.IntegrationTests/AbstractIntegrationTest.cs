// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE.txt in the project root for license information.

namespace Tvl.VisualStudio.MouseFastScroll.IntegrationTests
{
    using System;
    using System.Runtime.InteropServices;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows.Automation;
    using Microsoft.VisualStudio;
    using Microsoft.Win32.SafeHandles;
    using Tvl.VisualStudio.MouseFastScroll.IntegrationTests.Harness;
    using Xunit;
    using IMessageFilter = Microsoft.VisualStudio.OLE.Interop.IMessageFilter;
    using INTERFACEINFO = Microsoft.VisualStudio.OLE.Interop.INTERFACEINFO;
    using PENDINGMSG = Microsoft.VisualStudio.OLE.Interop.PENDINGMSG;
    using SERVERCALL = Microsoft.VisualStudio.OLE.Interop.SERVERCALL;

    [CaptureTestName]
    [Collection(nameof(SharedIntegrationHostFixture))]
    public abstract class AbstractIntegrationTest : IAsyncLifetime, IDisposable
    {
        private readonly MessageFilter _messageFilter;
        private readonly VisualStudioInstanceFactory _instanceFactory;
        private readonly Version _version;
        private VisualStudioInstanceContext _visualStudioContext;

        protected AbstractIntegrationTest(VisualStudioInstanceFactory instanceFactory, Version version)
        {
            Assert.Equal(ApartmentState.STA, Thread.CurrentThread.GetApartmentState());

            // Install a COM message filter to handle retry operations when the first attempt fails
            _messageFilter = RegisterMessageFilter();
            _instanceFactory = instanceFactory;
            _version = version;

            try
            {
                Automation.TransactionTimeout = 20000;
            }
            catch
            {
                _messageFilter.Dispose();
                throw;
            }
        }

        public VisualStudioInstance VisualStudio => _visualStudioContext?.Instance;

        public virtual async Task InitializeAsync()
        {
            try
            {
                _visualStudioContext = await _instanceFactory.GetNewOrUsedInstanceAsync(_version, SharedIntegrationHostFixture.RequiredPackageIds).ConfigureAwait(false);
            }
            catch
            {
                _messageFilter.Dispose();
                throw;
            }
        }

        public Task DisposeAsync()
        {
            return Task.FromResult<object>(null);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual MessageFilter RegisterMessageFilter()
            => new MessageFilter();

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                try
                {
                    _visualStudioContext?.Dispose();
                }
                finally
                {
                    _messageFilter.Dispose();
                }
            }
        }

        protected class MessageFilter : IMessageFilter, IDisposable
        {
            protected const uint CancelCall = ~0U;

            private readonly MessageFilterSafeHandle _messageFilterRegistration;
            private readonly TimeSpan _timeout;
            private readonly TimeSpan _retryDelay;

            public MessageFilter()
                : this(timeout: TimeSpan.FromSeconds(60), retryDelay: TimeSpan.FromMilliseconds(150))
            {
            }

            public MessageFilter(TimeSpan timeout, TimeSpan retryDelay)
            {
                _timeout = timeout;
                _retryDelay = retryDelay;
                _messageFilterRegistration = MessageFilterSafeHandle.Register(this);
            }

            public virtual uint HandleInComingCall(uint dwCallType, IntPtr htaskCaller, uint dwTickCount, INTERFACEINFO[] lpInterfaceInfo)
            {
                return (uint)SERVERCALL.SERVERCALL_ISHANDLED;
            }

            public virtual uint RetryRejectedCall(IntPtr htaskCallee, uint dwTickCount, uint dwRejectType)
            {
                if ((SERVERCALL)dwRejectType != SERVERCALL.SERVERCALL_RETRYLATER
                    && (SERVERCALL)dwRejectType != SERVERCALL.SERVERCALL_REJECTED)
                {
                    return CancelCall;
                }

                if (dwTickCount >= _timeout.TotalMilliseconds)
                {
                    return CancelCall;
                }

                return (uint)_retryDelay.TotalMilliseconds;
            }

            public virtual uint MessagePending(IntPtr htaskCallee, uint dwTickCount, uint dwPendingType)
            {
                return (uint)PENDINGMSG.PENDINGMSG_WAITDEFPROCESS;
            }

            protected virtual void Dispose(bool disposing)
            {
                if (disposing)
                {
                    _messageFilterRegistration.Dispose();
                }
            }

            public void Dispose()
            {
                Dispose(true);
                GC.SuppressFinalize(this);
            }
        }

        private sealed class MessageFilterSafeHandle : SafeHandleMinusOneIsInvalid
        {
            private readonly IntPtr _oldFilter;

            private MessageFilterSafeHandle(IntPtr handle)
                : base(true)
            {
                SetHandle(handle);

                try
                {
                    if (CoRegisterMessageFilter(handle, out _oldFilter) != VSConstants.S_OK)
                    {
                        throw new InvalidOperationException("Failed to register a new message filter");
                    }
                }
                catch
                {
                    SetHandleAsInvalid();
                    throw;
                }
            }

            [DllImport("ole32", SetLastError = true)]
            private static extern int CoRegisterMessageFilter(IntPtr messageFilter, out IntPtr oldMessageFilter);

            public static MessageFilterSafeHandle Register<T>(T messageFilter)
                where T : IMessageFilter
            {
                var handle = Marshal.GetComInterfaceForObject<T, IMessageFilter>(messageFilter);
                return new MessageFilterSafeHandle(handle);
            }

            protected override bool ReleaseHandle()
            {
                if (CoRegisterMessageFilter(_oldFilter, out _) == VSConstants.S_OK)
                {
                    Marshal.Release(handle);
                }

                return true;
            }
        }
    }
}
