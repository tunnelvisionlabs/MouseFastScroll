// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE.txt in the project root for license information.

namespace Tvl.VisualStudio.MouseFastScroll.IntegrationTests.Harness
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.VisualStudio.Extensibility.Testing;
    using WindowsInput;
    using WindowsInput.Native;

    [TestService]
    internal partial class IdeSendKeys : InProcComponent
    {
        internal async Task SendAsync(params object[] keys)
        {
            await SendAsync(
                inputSimulator =>
                {
                    foreach (var key in keys)
                    {
                        switch (key)
                        {
                        case string str:
                            var text = str.Replace("\r\n", "\r").Replace("\n", "\r");
                            int index = 0;
                            while (index < text.Length)
                            {
                                if (text[index] == '\r')
                                {
                                    inputSimulator.Keyboard.KeyPress(VirtualKeyCode.RETURN);
                                    index++;
                                }
                                else
                                {
                                    int nextIndex = text.IndexOf('\r', index);
                                    if (nextIndex == -1)
                                    {
                                        nextIndex = text.Length;
                                    }

                                    inputSimulator.Keyboard.TextEntry(text.Substring(index, nextIndex - index));
                                    index = nextIndex;
                                }
                            }

                            break;

                        case char c:
                            inputSimulator.Keyboard.TextEntry(c);
                            break;

                        case VirtualKeyCode virtualKeyCode:
                            inputSimulator.Keyboard.KeyPress(virtualKeyCode);
                            break;

                        case null:
                            throw new ArgumentNullException(nameof(keys));

                        default:
                            throw new ArgumentException($"Unexpected type encountered: {key.GetType()}", nameof(keys));
                        }
                    }
                },
                CancellationToken.None);
        }

        internal async Task SendAsync(Action<InputSimulator> actions, CancellationToken cancellationToken)
        {
            if (actions == null)
            {
                throw new ArgumentNullException(nameof(actions));
            }

            await TestServices.Editor.ActivateAsync(cancellationToken);

            await Task.Run(() => actions(new InputSimulator()));

            await WaitForApplicationIdleAsync(cancellationToken);
        }
    }
}
