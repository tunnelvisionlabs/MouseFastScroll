// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE.txt in the project root for license information.

namespace Tvl.VisualStudio.MouseFastScroll.IntegrationTests
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Media;
    using Microsoft.VisualStudio.Text.Formatting;
    using Tvl.VisualStudio.MouseFastScroll.IntegrationTests.Harness;
    using Tvl.VisualStudio.MouseFastScroll.IntegrationTests.InProcess;
    using WindowsInput.Native;
    using Xunit;
    using Xunit.Abstractions;
    using _DTE = EnvDTE._DTE;
    using DTE = EnvDTE.DTE;
    using vsSaveChanges = EnvDTE.vsSaveChanges;

    public class ScrollingIntegrationTest : AbstractIdeIntegrationTest
    {
        public ScrollingIntegrationTest(ITestOutputHelper testOutputHelper)
        {
            TestOutputHelper = testOutputHelper;
            Editor = new Editor_InProc2(JoinableTaskFactory);
            SendKeys = new IdeSendKeys(JoinableTaskFactory);
        }

        protected ITestOutputHelper TestOutputHelper
        {
            get;
        }

        private Editor_InProc2 Editor
        {
            get;
        }

        private IdeSendKeys SendKeys
        {
            get;
        }

        [VsFact]
        public async Task BasicScrollingBehaviorAsync()
        {
            var dte = (DTE)ServiceProvider.GetService(typeof(_DTE));
            var window = dte.ItemOperations.NewFile(Name: Guid.NewGuid() + ".txt");

            string initialText = string.Join(string.Empty, Enumerable.Range(0, 400).Select(i => Guid.NewGuid() + Environment.NewLine));
            await Editor.SetTextAsync(initialText);

            string additionalTypedText = Guid.NewGuid().ToString() + "\n" + Guid.NewGuid().ToString();
            await Editor.ActivateAsync();
            await SendKeys.SendAsync(additionalTypedText);

            string expected = initialText + additionalTypedText.Replace("\n", Environment.NewLine);
            Assert.Equal(expected, await Editor.GetTextAsync());

            Assert.Equal(expected.Length, await Editor.GetCaretPositionAsync());

            // Move the caret and verify the final position. Note that the MoveCaret operation does not scroll the view.
            int firstVisibleLine = await Editor.GetFirstVisibleLineAsync();
            Assert.True(firstVisibleLine > 0, "Expected the view to start after the first line at this point.");
            await Editor.MoveCaretAsync(0);
            Assert.Equal(0, await Editor.GetCaretPositionAsync());
            Assert.Equal(firstVisibleLine, await Editor.GetFirstVisibleLineAsync());

            await SendKeys.SendAsync(inputSimulator =>
            {
                inputSimulator.Keyboard
                    .KeyDown(VirtualKeyCode.CONTROL)
                    .KeyPress(VirtualKeyCode.HOME)
                    .KeyUp(VirtualKeyCode.CONTROL);
            });

            Assert.True(await Editor.IsCaretOnScreenAsync());
            firstVisibleLine = await Editor.GetFirstVisibleLineAsync();
            Assert.Equal(0, firstVisibleLine);

            int lastVisibleLine = await Editor.GetLastVisibleLineAsync();
            var lastVisibleLineState = await Editor.GetLastVisibleLineStateAsync();
            Assert.True(firstVisibleLine < lastVisibleLine);

            Point point = await Editor.GetCenterOfEditorOnScreenAsync();

            await MoveMouseAsync(point);
            await SendKeys.SendAsync(inputSimulator => inputSimulator.Mouse.VerticalScroll(-1));

            Assert.Equal(0, await Editor.GetCaretPositionAsync());
            Assert.Equal(3, await Editor.GetFirstVisibleLineAsync());

            await MoveMouseAsync(point);
            await SendKeys.SendAsync(inputSimulator => inputSimulator.Mouse.VerticalScroll(1));

            Assert.Equal(0, await Editor.GetCaretPositionAsync());
            Assert.Equal(0, await Editor.GetFirstVisibleLineAsync());

            await MoveMouseAsync(point);
            await SendKeys.SendAsync(inputSimulator =>
            {
                inputSimulator
                    .Keyboard.KeyDown(VirtualKeyCode.CONTROL)
                    .Mouse.VerticalScroll(-1)
                    .Keyboard.Sleep(10).KeyUp(VirtualKeyCode.CONTROL);
            });

            int expectedLastVisibleLine = lastVisibleLine + (lastVisibleLineState == VisibilityState.FullyVisible ? 1 : 0);
            Assert.Equal(0, await Editor.GetCaretPositionAsync());
            Assert.Equal(expectedLastVisibleLine, await Editor.GetFirstVisibleLineAsync());

            await MoveMouseAsync(point);
            await SendKeys.SendAsync(inputSimulator =>
            {
                inputSimulator
                    .Keyboard.KeyDown(VirtualKeyCode.CONTROL)
                    .Mouse.VerticalScroll(1)
                    .Keyboard.Sleep(10).KeyUp(VirtualKeyCode.CONTROL);
            });

            Assert.Equal(0, await Editor.GetCaretPositionAsync());
            Assert.Equal(0, await Editor.GetFirstVisibleLineAsync());

            window.Close(vsSaveChanges.vsSaveChangesNo);
        }

        /// <summary>
        /// Verifies that the Ctrl+Scroll operations do not change the zoom level in the editor.
        /// </summary>
        [VsFact]
        public async Task ZoomDisabledAsync()
        {
            var dte = (DTE)ServiceProvider.GetService(typeof(_DTE));
            var window = dte.ItemOperations.NewFile(Name: Guid.NewGuid() + ".txt");

            string initialText = string.Join(string.Empty, Enumerable.Range(0, 400).Select(i => Guid.NewGuid() + Environment.NewLine));
            await Editor.SetTextAsync(initialText);

            string additionalTypedText = Guid.NewGuid().ToString() + "\n" + Guid.NewGuid().ToString();
            await SendKeys.SendAsync(additionalTypedText);

            string expected = initialText + additionalTypedText.Replace("\n", Environment.NewLine);
            Assert.Equal(expected, await Editor.GetTextAsync());

            Assert.Equal(expected.Length, await Editor.GetCaretPositionAsync());

            await SendKeys.SendAsync(inputSimulator =>
            {
                inputSimulator.Keyboard
                    .KeyDown(VirtualKeyCode.CONTROL)
                    .KeyPress(VirtualKeyCode.HOME)
                    .KeyUp(VirtualKeyCode.CONTROL);
            });

            int firstVisibleLine = await Editor.GetFirstVisibleLineAsync();
            Assert.Equal(0, firstVisibleLine);

            int lastVisibleLine = await Editor.GetLastVisibleLineAsync();
            var lastVisibleLineState = await Editor.GetLastVisibleLineStateAsync();
            Assert.True(firstVisibleLine < lastVisibleLine);

            double zoomLevel = await Editor.GetZoomLevelAsync();

            Point point = await Editor.GetCenterOfEditorOnScreenAsync();

            await MoveMouseAsync(point);
            await SendKeys.SendAsync(inputSimulator =>
            {
                inputSimulator
                    .Keyboard.KeyDown(VirtualKeyCode.CONTROL)
                    .Mouse.VerticalScroll(-1)
                    .Keyboard.Sleep(10).KeyUp(VirtualKeyCode.CONTROL);
            });

            int expectedLastVisibleLine = lastVisibleLine + (lastVisibleLineState == VisibilityState.FullyVisible ? 1 : 0);
            Assert.Equal(0, await Editor.GetCaretPositionAsync());
            Assert.Equal(expectedLastVisibleLine, await Editor.GetFirstVisibleLineAsync());
            Assert.Equal(zoomLevel, await Editor.GetZoomLevelAsync());

            await MoveMouseAsync(point);
            await SendKeys.SendAsync(inputSimulator =>
            {
                inputSimulator
                    .Keyboard.KeyDown(VirtualKeyCode.CONTROL)
                    .Mouse.VerticalScroll(1)
                    .Keyboard.Sleep(10).KeyUp(VirtualKeyCode.CONTROL);
            });

            Assert.Equal(0, await Editor.GetCaretPositionAsync());
            Assert.Equal(0, await Editor.GetFirstVisibleLineAsync());
            Assert.Equal(zoomLevel, await Editor.GetZoomLevelAsync());

            window.Close(vsSaveChanges.vsSaveChangesNo);
        }

        private async Task MoveMouseAsync(Point point)
        {
            TestOutputHelper.WriteLine($"Moving mouse to ({point.X}, {point.Y}).");
            int horizontalResolution = NativeMethods.GetSystemMetrics(NativeMethods.SM_CXSCREEN);
            int verticalResolution = NativeMethods.GetSystemMetrics(NativeMethods.SM_CYSCREEN);
            var virtualPoint = new ScaleTransform(65535.0 / horizontalResolution, 65535.0 / verticalResolution).Transform(point);
            TestOutputHelper.WriteLine($"Screen resolution of ({horizontalResolution}, {verticalResolution}) translates mouse to ({virtualPoint.X}, {virtualPoint.Y}).");

            await SendKeys.SendAsync(inputSimulator => inputSimulator.Mouse.MoveMouseTo(virtualPoint.X, virtualPoint.Y));

            // ⚠ The call to GetCursorPos is required for correct behavior.
            var actualPoint = NativeMethods.GetCursorPos();
            Assert.True(Math.Abs(actualPoint.X - point.X) <= 1);
            Assert.True(Math.Abs(actualPoint.Y - point.Y) <= 1);
        }
    }
}
