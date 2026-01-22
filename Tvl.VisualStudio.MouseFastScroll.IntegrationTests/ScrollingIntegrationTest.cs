// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE.txt in the project root for license information.

namespace Tvl.VisualStudio.MouseFastScroll.IntegrationTests
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Media;
    using Microsoft.VisualStudio.Extensibility.Testing;
    using Microsoft.VisualStudio.Shell.Interop;
    using Microsoft.VisualStudio.Text.Formatting;
    using Tvl.VisualStudio.MouseFastScroll.IntegrationTests.Harness;
    using WindowsInput.Native;
    using Xunit;
    using Xunit.Abstractions;
    using DTE = EnvDTE.DTE;
    using vsSaveChanges = EnvDTE.vsSaveChanges;

    public class ScrollingIntegrationTest : AbstractIdeIntegrationTest
    {
        public ScrollingIntegrationTest(ITestOutputHelper testOutputHelper)
        {
            TestOutputHelper = testOutputHelper;
        }

        protected ITestOutputHelper TestOutputHelper
        {
            get;
        }

        [IdeFact]
        public async Task BasicScrollingBehaviorAsync()
        {
            var dte = await TestServices.Shell.GetRequiredGlobalServiceAsync<SDTE, DTE>(HangMitigatingCancellationToken);
            var window = dte.ItemOperations.NewFile(Name: Guid.NewGuid() + ".txt");

            string initialText = string.Join(string.Empty, Enumerable.Range(0, 400).Select(i => Guid.NewGuid() + Environment.NewLine));
            await TestServices.Editor.SetTextAsync(initialText, HangMitigatingCancellationToken);

            string additionalTypedText = Guid.NewGuid().ToString() + "\n" + Guid.NewGuid().ToString();
            await TestServices.Editor.ActivateAsync(HangMitigatingCancellationToken);
            await TestServices.IdeSendKeys.SendAsync(additionalTypedText);

            string expected = initialText + additionalTypedText.Replace("\n", Environment.NewLine);
            Assert.Equal(expected, await TestServices.Editor.GetTextAsync(HangMitigatingCancellationToken));

            Assert.Equal(expected.Length, await TestServices.Editor.GetCaretPositionAsync(HangMitigatingCancellationToken));

            // Move the caret and verify the final position. Note that the MoveCaret operation does not scroll the view.
            int firstVisibleLine = await TestServices.Editor.GetFirstVisibleLineAsync(HangMitigatingCancellationToken);
            Assert.True(firstVisibleLine > 0, "Expected the view to start after the first line at this point.");
            await TestServices.Editor.MoveCaretAsync(0, HangMitigatingCancellationToken);
            Assert.Equal(0, await TestServices.Editor.GetCaretPositionAsync(HangMitigatingCancellationToken));
            Assert.Equal(firstVisibleLine, await TestServices.Editor.GetFirstVisibleLineAsync(HangMitigatingCancellationToken));

            await TestServices.IdeSendKeys.SendAsync(
                inputSimulator =>
                {
                    inputSimulator.Keyboard
                        .KeyDown(VirtualKeyCode.CONTROL)
                        .KeyPress(VirtualKeyCode.HOME)
                        .KeyUp(VirtualKeyCode.CONTROL);
                },
                HangMitigatingCancellationToken);

            Assert.True(await TestServices.Editor.IsCaretOnScreenAsync(HangMitigatingCancellationToken));
            firstVisibleLine = await TestServices.Editor.GetFirstVisibleLineAsync(HangMitigatingCancellationToken);
            Assert.Equal(0, firstVisibleLine);

            int lastVisibleLine = await TestServices.Editor.GetLastVisibleLineAsync(HangMitigatingCancellationToken);
            var lastVisibleLineState = await TestServices.Editor.GetLastVisibleLineStateAsync(HangMitigatingCancellationToken);
            Assert.True(firstVisibleLine < lastVisibleLine);

            Point point = await TestServices.Editor.GetCenterOfEditorOnScreenAsync(HangMitigatingCancellationToken);

            await MoveMouseAsync(point);
            await TestServices.IdeSendKeys.SendAsync(inputSimulator => inputSimulator.Mouse.VerticalScroll(-1), HangMitigatingCancellationToken);

            Assert.Equal(0, await TestServices.Editor.GetCaretPositionAsync(HangMitigatingCancellationToken));
            Assert.Equal(3, await TestServices.Editor.GetFirstVisibleLineAsync(HangMitigatingCancellationToken));

            await MoveMouseAsync(point);
            await TestServices.IdeSendKeys.SendAsync(inputSimulator => inputSimulator.Mouse.VerticalScroll(1), HangMitigatingCancellationToken);

            Assert.Equal(0, await TestServices.Editor.GetCaretPositionAsync(HangMitigatingCancellationToken));
            Assert.Equal(0, await TestServices.Editor.GetFirstVisibleLineAsync(HangMitigatingCancellationToken));

            await MoveMouseAsync(point);
            await TestServices.IdeSendKeys.SendAsync(
                inputSimulator =>
                {
                    inputSimulator
                        .Keyboard.KeyDown(VirtualKeyCode.CONTROL)
                        .Mouse.VerticalScroll(-1)
                        .Keyboard.Sleep(10).KeyUp(VirtualKeyCode.CONTROL);
                },
                HangMitigatingCancellationToken);

            int expectedLastVisibleLine = lastVisibleLine + (lastVisibleLineState == VisibilityState.FullyVisible ? 1 : 0);
            Assert.Equal(0, await TestServices.Editor.GetCaretPositionAsync(HangMitigatingCancellationToken));
            Assert.Equal(expectedLastVisibleLine, await TestServices.Editor.GetFirstVisibleLineAsync(HangMitigatingCancellationToken));

            await MoveMouseAsync(point);
            await TestServices.IdeSendKeys.SendAsync(
                inputSimulator =>
                {
                    inputSimulator
                        .Keyboard.KeyDown(VirtualKeyCode.CONTROL)
                        .Mouse.VerticalScroll(1)
                        .Keyboard.Sleep(10).KeyUp(VirtualKeyCode.CONTROL);
                },
                HangMitigatingCancellationToken);

            Assert.Equal(0, await TestServices.Editor.GetCaretPositionAsync(HangMitigatingCancellationToken));
            Assert.Equal(0, await TestServices.Editor.GetFirstVisibleLineAsync(HangMitigatingCancellationToken));

            window.Close(vsSaveChanges.vsSaveChangesNo);
        }

        /// <summary>
        /// Verifies that the Ctrl+Scroll operations do not change the zoom level in the editor.
        /// </summary>
        [IdeFact]
        public async Task ZoomDisabledAsync()
        {
            var dte = await TestServices.Shell.GetRequiredGlobalServiceAsync<SDTE, DTE>(HangMitigatingCancellationToken);
            var window = dte.ItemOperations.NewFile(Name: Guid.NewGuid() + ".txt");

            string initialText = string.Join(string.Empty, Enumerable.Range(0, 400).Select(i => Guid.NewGuid() + Environment.NewLine));
            await TestServices.Editor.SetTextAsync(initialText, HangMitigatingCancellationToken);

            string additionalTypedText = Guid.NewGuid().ToString() + "\n" + Guid.NewGuid().ToString();
            await TestServices.IdeSendKeys.SendAsync(additionalTypedText);

            string expected = initialText + additionalTypedText.Replace("\n", Environment.NewLine);
            Assert.Equal(expected, await TestServices.Editor.GetTextAsync(HangMitigatingCancellationToken));

            Assert.Equal(expected.Length, await TestServices.Editor.GetCaretPositionAsync(HangMitigatingCancellationToken));

            await TestServices.IdeSendKeys.SendAsync(
                inputSimulator =>
                {
                    inputSimulator.Keyboard
                        .KeyDown(VirtualKeyCode.CONTROL)
                        .KeyPress(VirtualKeyCode.HOME)
                        .KeyUp(VirtualKeyCode.CONTROL);
                },
                HangMitigatingCancellationToken);

            int firstVisibleLine = await TestServices.Editor.GetFirstVisibleLineAsync(HangMitigatingCancellationToken);
            Assert.Equal(0, firstVisibleLine);

            int lastVisibleLine = await TestServices.Editor.GetLastVisibleLineAsync(HangMitigatingCancellationToken);
            var lastVisibleLineState = await TestServices.Editor.GetLastVisibleLineStateAsync(HangMitigatingCancellationToken);
            Assert.True(firstVisibleLine < lastVisibleLine);

            double zoomLevel = await TestServices.Editor.GetZoomLevelAsync(HangMitigatingCancellationToken);

            Point point = await TestServices.Editor.GetCenterOfEditorOnScreenAsync(HangMitigatingCancellationToken);

            await MoveMouseAsync(point);
            await TestServices.IdeSendKeys.SendAsync(
                inputSimulator =>
                {
                    inputSimulator
                        .Keyboard.KeyDown(VirtualKeyCode.CONTROL)
                        .Mouse.VerticalScroll(-1)
                        .Keyboard.Sleep(10).KeyUp(VirtualKeyCode.CONTROL);
                },
                HangMitigatingCancellationToken);

            int expectedLastVisibleLine = lastVisibleLine + (lastVisibleLineState == VisibilityState.FullyVisible ? 1 : 0);
            Assert.Equal(0, await TestServices.Editor.GetCaretPositionAsync(HangMitigatingCancellationToken));
            Assert.Equal(expectedLastVisibleLine, await TestServices.Editor.GetFirstVisibleLineAsync(HangMitigatingCancellationToken));
            Assert.Equal(zoomLevel, await TestServices.Editor.GetZoomLevelAsync(HangMitigatingCancellationToken));

            await MoveMouseAsync(point);
            await TestServices.IdeSendKeys.SendAsync(
                inputSimulator =>
                {
                    inputSimulator
                        .Keyboard.KeyDown(VirtualKeyCode.CONTROL)
                        .Mouse.VerticalScroll(1)
                        .Keyboard.Sleep(10).KeyUp(VirtualKeyCode.CONTROL);
                },
                HangMitigatingCancellationToken);

            Assert.Equal(0, await TestServices.Editor.GetCaretPositionAsync(HangMitigatingCancellationToken));
            Assert.Equal(0, await TestServices.Editor.GetFirstVisibleLineAsync(HangMitigatingCancellationToken));
            Assert.Equal(zoomLevel, await TestServices.Editor.GetZoomLevelAsync(HangMitigatingCancellationToken));

            window.Close(vsSaveChanges.vsSaveChangesNo);
        }

        private async Task MoveMouseAsync(Point point)
        {
            TestOutputHelper.WriteLine($"Moving mouse to ({point.X}, {point.Y}).");
            int horizontalResolution = NativeMethods.GetSystemMetrics(NativeMethods.SM_CXSCREEN);
            int verticalResolution = NativeMethods.GetSystemMetrics(NativeMethods.SM_CYSCREEN);
            var virtualPoint = new ScaleTransform(65535.0 / horizontalResolution, 65535.0 / verticalResolution).Transform(point);
            TestOutputHelper.WriteLine($"Screen resolution of ({horizontalResolution}, {verticalResolution}) translates mouse to ({virtualPoint.X}, {virtualPoint.Y}).");

            await TestServices.IdeSendKeys.SendAsync(inputSimulator => inputSimulator.Mouse.MoveMouseTo(virtualPoint.X, virtualPoint.Y), HangMitigatingCancellationToken);

            // ⚠ The call to GetCursorPos is required for correct behavior.
            var actualPoint = NativeMethods.GetCursorPos();
            Assert.True(Math.Abs(actualPoint.X - point.X) <= 1);
            Assert.True(Math.Abs(actualPoint.Y - point.Y) <= 1);
        }
    }
}
