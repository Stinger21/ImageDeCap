using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.IO;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace imageDeCap
{
    public partial class ImageEditor : Form
    {
        public enum EditorResult
        {
            Quit,
            Upload,
            Clipboard,
            Save,
        }

        PictureEditor Editor;
        public Button CurrentSwatch;
        EditorResult result = EditorResult.Quit;

        public (EditorResult output, byte[] Data) FinalFunction()
        {
            byte[] OutputData;
            
            if (result == EditorResult.Quit)
            {
                OutputData = Utilities.GetBytes(Editor.OriginalImage, ImageFormat.Png);
            }
            else
            {
                OutputData = Utilities.GetBytes(Editor.EditedImage, ImageFormat.Png);
                Clipboard.Clear();
            }
            return (result, OutputData);
        }
        Rectangle SelectedRegion;
        public ImageEditor(byte[] ImageData, int X, int Y, Rectangle SelectedRegion)//on start
        {
            InitializeComponent();
            this.SelectedRegion = SelectedRegion;
            System.Threading.Thread.Sleep(100);

            Editor = new PictureEditor(Image.FromStream(new MemoryStream(ImageData)), this);

            int Width;
            int Height;
            if (Editor.EditedImage.Width < 325)
                Width = 325;
            else
                Width = Editor.EditedImage.Width;
            if (Editor.EditedImage.Height < 253)
                Height = 253;
            else
                Height = Editor.EditedImage.Height;

            this.Size = new Size(Width + 23-5, Height + 95 - 15);
            this.StartPosition = FormStartPosition.Manual;
            this.Location = new Point(X - 7, Y - 20);

            ImageContainer.Image = Editor.TempImage;

            UploadButton.Enabled = !Preferences.NeverUpload || Preferences.uploadToFTP; // enabled if we upload to ftp or imgur
            if (Preferences.NeverUpload)
            {
                this.AcceptButton = ClipboardButton;
                this.ActiveControl = ClipboardButton;
            }
            else
            {
                this.AcceptButton = UploadButton;
                this.ActiveControl = UploadButton;
            }
            CurrentSwatch = FrontSwatch;
        }


        // Drawing

        private void ImageContainer_MouseDown(object sender, MouseEventArgs e){ImageContainer_MouseMove(sender, e);}
        private void NewImageEditor_MouseClick(object sender, MouseEventArgs e){ImageContainer_MouseClick(sender, e);}
        private void Panel1_MouseClick(object sender, MouseEventArgs e){ImageContainer_MouseClick(sender, e);}
        private void ImageContainer_MouseClick(object sender, MouseEventArgs e){ImageContainer_MouseMove(sender, e);}
        private void ImageContainer_MouseMove(object sender, MouseEventArgs e)
        {
            Point mousePos = ImageContainer.PointToClient(Cursor.Position);

            Editor.LMBIsDown = e.Button == MouseButtons.Left;
            Editor.RMBIsDown = e.Button == MouseButtons.Right;

            Editor.Alt = false;
            Editor.Ctrl = false;
            Editor.Shift = false;
            if (System.Windows.Input.Keyboard.IsKeyDown(System.Windows.Input.Key.LeftAlt))
                Editor.Alt = true;
            if (System.Windows.Input.Keyboard.IsKeyDown(System.Windows.Input.Key.LeftCtrl))
                Editor.Ctrl = true;
            if (System.Windows.Input.Keyboard.IsKeyDown(System.Windows.Input.Key.LeftShift))
                Editor.Shift = true;

            Editor.MouseMove(new Vector2(mousePos));

            if (Editor.LastLMBIsDown != Editor.LMBIsDown)
            {
                if(Editor.LMBIsDown)
                    Editor.LMBDown(new Vector2(mousePos));
                else
                    Editor.LMBUp(new Vector2(mousePos));
                Editor.LastLMBIsDown = Editor.LMBIsDown;
                return;
            }

            if (Editor.LastRMBIsDown != Editor.RMBIsDown)
            {
                if (Editor.RMBIsDown)
                    Editor.RMBDown(new Vector2(mousePos));
                else
                    Editor.RMBUp();
                Editor.LastRMBIsDown = Editor.RMBIsDown;
                return;
            }
        }
        

        // Hotkeys

        private void ImageEditor_KeyDown(object sender, KeyEventArgs e)
        {
            string KeyCode = e.KeyCode.ToString();

            if (KeyCode == "Escape")
            {
                // When the user hits escape we want to put whatever they had drawn into the clipboard.
                // I like it fight me.
                result = EditorResult.Clipboard;
                this.Close();
                return;
            }
            if(TextFieldInput.ContainsFocus)
            {
                return;
            }
            Vector2 mousePos = new Vector2(ImageContainer.PointToClient(Cursor.Position));

            switch (KeyCode)
            {
                case "Z":
                    if (e.Control)
                        Editor.Undo();
                    break;
                case "C":
                    if (e.Control)
                        Clipboard.SetImage(Editor.EditedImage);
                    break;
                case "T":
                    AddTextButton_Click(null, null);
                    break;
                case "A":
                    Editor.SetState(mousePos, PictureEditor.EditorState.Arrow);
                    break;
                case "B":
                    Editor.SetState(mousePos, PictureEditor.EditorState.Box);
                    break;
                case "X":
                    CurrentSwatch = (CurrentSwatch == BackSwatch) ? FrontSwatch : BackSwatch;
                    CurrentSwatch.BringToFront();
                    break;
            }
        }
        
        // Buttons
        private void UploadButton_click(object sender, EventArgs e)
        {
            result = EditorResult.Upload;
            Close();
        }
        private void ClipboardButton_click(object sender, EventArgs e)
        {
            result = EditorResult.Clipboard;
            Close();
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            result = EditorResult.Save;
            Close();
        }

        private void UndoButton_Click(object sender, EventArgs e)
        {
            Editor.Undo();
        }

        private void CurrentColor_Click(object sender, EventArgs e)
        {
            var ClickedButton = (sender as Button);

            if (ClickedButton == CurrentSwatch)
            {
                DialogResult r = colorDialog1.ShowDialog();
                if(r == DialogResult.OK)
                {
                    ClickedButton.BackColor = colorDialog1.Color;
                    CurrentSwatch = ClickedButton;
                }
            }
            else
            {
                CurrentSwatch = ClickedButton;
                ClickedButton.BringToFront();
            }
        }
        
        private void TextFieldInput_TextChanged(object sender, EventArgs e)
        {
            Editor.DrawTempText();
            ImageContainer.Refresh();
        }

        private void TextFieldInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Back)
            {
                if (e.Control)
                {
                    int lastSpace = TextFieldInput.Text.LastIndexOf(' ');
                    if (lastSpace > 0)
                    {
                        TextFieldInput.Text = TextFieldInput.Text.Substring(0, lastSpace);
                        TextFieldInput.Text = TextFieldInput.Text.Replace('\u007f', ' ');
                        TextFieldInput.Select(TextFieldInput.Text.Length, 0);
                    }
                    else
                    {
                        TextFieldInput.Text = "";
                    }
                }
            }
        }
        
        private void AddTextButton_Click(object sender, EventArgs e)
        {
            TextFieldInput.Focus();
            TextFieldInput.SelectAll();
            Editor.SetState(new Vector2(ImageContainer.PointToClient(Cursor.Position)), PictureEditor.EditorState.Text);
        }
        private void PickColorButton_click(object sender, EventArgs e)
        {
            ImageContainer.Cursor = Cursors.Cross;
            Editor.SetState(new Vector2(ImageContainer.PointToClient(Cursor.Position)), PictureEditor.EditorState.Picking);
        }
        private void AddArrowButton_Click(object sender, EventArgs e)
        {
            Editor.SetState(new Vector2(ImageContainer.PointToClient(Cursor.Position)), PictureEditor.EditorState.Arrow);
        }
        private void AddBoxButton_Click(object sender, EventArgs e)
        {
            Editor.SetState(new Vector2(ImageContainer.PointToClient(Cursor.Position)), PictureEditor.EditorState.Box);
        }
        private void HelpButton_Click(object sender, EventArgs e)
        {
            //InfoText.Visible = !InfoText.Visible;
        }

        private void CaptureAgain_Click(object sender, EventArgs e)
        {
            Utilities.PlaySound("snip.wav");
            Bitmap result = ScreenCapturer.Capture(
                ScreenCaptureMode.Bounds,
                SelectedRegion.X,
                SelectedRegion.Y,
                SelectedRegion.Width + 1,
                SelectedRegion.Height + 1);
            
            ScreenCapturer.UploadImageData(Utilities.GetBytes(result, ImageFormat.Png), Filetype.png, false, false, null, SelectedRegion);
        }
    }

    // Split the actual drawing into this class because it was getting sphagettied with the UI & saving code.
    public class PictureEditor
    {
        private EditorState State = EditorState.Drawing;
        List<Bitmap> UndoHistory = new List<Bitmap>();
        public Image EditedImage;
        public Image TempImage;
        public Image OriginalImage;
        ImageEditor Owner;

        public bool Ctrl;
        public bool Shift;
        public bool Alt;
        public bool RMBIsDown;
        public bool LMBIsDown;
        public bool LastRMBIsDown;
        public bool LastLMBIsDown;

        public float BrushSize = 20.0f;
        public float TextSize = 35.0f;
        public float GammaCorrectedBrushSize = 20.0f;
        public float GammaCorrectedTextSize = 35.0f;

        Vector2 ItemStartPosition;

        Vector2 BrushLocation;
        Vector2 LastBrushLocation;
        Vector2 MousePosition;

        Vector2 MouseDownLocation;

        float BrushDelta = 0;
        float LastBrushValue = 0;

        // ctor
        public PictureEditor(Image InputImage, ImageEditor owner)
        {
            this.Owner = owner;
            DrawImage(ref InputImage, ref OriginalImage);
            DrawImage(ref InputImage, ref TempImage);
            DrawImage(ref InputImage, ref EditedImage);

            Image FirstImage = null;
            DrawImage(ref InputImage, ref FirstImage);
            UndoHistory.Add((Bitmap)FirstImage);
            SetState(new Vector2(0, 0), EditorState.Drawing);
        }

        public enum EditorState
        {
            Drawing,
            Text,
            Picking,
            Box,
            Arrow,
        }

        public EditorState GetState()
        {
            return this.State;
        }

        public void SetState(Vector2 Position, EditorState State)
        {
            ItemStartPosition = Position;
            this.State = State;
            
            Owner.TextButton.Enabled    = true;
            Owner.TextButton.Enabled    = true;
            Owner.BoxButton.Enabled     = true;
            Owner.ArrowButton.Enabled   = true;

            Owner.TextButton.BackColor  = SystemColors.Control;
            Owner.TextButton.BackColor  = SystemColors.Control;
            Owner.BoxButton.BackColor   = SystemColors.Control;
            Owner.ArrowButton.BackColor = SystemColors.Control;

            switch (State)
            {
                case EditorState.Drawing:
                    break;
                case EditorState.Text:
                    Owner.TextButton.Enabled = false;
                    Owner.TextButton.BackColor = SystemColors.ControlDark;
                    break;
                case EditorState.Picking:
                    Owner.PickButton.Enabled = false;
                    Owner.PickButton.BackColor = SystemColors.ControlDark;
                    break;
                case EditorState.Box:
                    Owner.BoxButton.Enabled = false;
                    Owner.BoxButton.BackColor = SystemColors.ControlDark;
                    break;
                case EditorState.Arrow:
                    Owner.ArrowButton.Enabled = false;
                    Owner.ArrowButton.BackColor = SystemColors.ControlDark;
                    break;
                default:
                    break;
            }
        }

        public void LMBDown(Vector2 Position)
        {
            LMBIsDown = true;

            if (Shift)
                DrawLine(EditedImage, LastBrushLocation, Position);

            BrushLocation = Position;
            LastBrushLocation = Position;
            MousePosition = Position;

            if (State == EditorState.Box)
            {
                ItemStartPosition = Position;
            }
            else if (State == EditorState.Arrow)
            {
                ItemStartPosition = Position;
            }
            if (State == EditorState.Drawing)
            {
                DrawImage(ref EditedImage, ref TempImage);
            }
        }
        
        public void RMBDown(Vector2 Position)
        {
            RMBIsDown = true;
            MouseDownLocation = Position;
            LastBrushValue = 0;
            BrushDelta = 0;
        }


        public void DrawTempText()
        {
            DrawImage(ref EditedImage, ref TempImage);
            DrawText(TempImage, Owner.TextFieldInput.Text, MousePosition);
        }

        public void MouseMove(Vector2 Position)
        {
            if (!RMBIsDown)
            {
                MousePosition = Position;
            }
            if (State == EditorState.Drawing)
            {
                if (LMBIsDown && LastLMBIsDown)
                {
                    float Distance = Vector2.Distance(BrushLocation, MousePosition);
                    if (Distance > Preferences.BrushSmoothingDistance)
                    {
                        BrushLocation = Vector2.MoveTowards(BrushLocation, MousePosition, Distance - Preferences.BrushSmoothingDistance);
                        DrawLine(TempImage, LastBrushLocation, BrushLocation, Owner.CurrentSwatch.BackColor);
                    }
                    LastBrushLocation = BrushLocation;
                }
                else if(!LMBIsDown && LastLMBIsDown) // Mouse up
                {
                    DrawLine(TempImage, LastBrushLocation, MousePosition, Owner.CurrentSwatch.BackColor);
                }
                else if (RMBIsDown && LastRMBIsDown)
                {
                    float BrushValue = (Position.X - MouseDownLocation.X) + (Position.Y - MouseDownLocation.Y);
                    BrushDelta = BrushValue - LastBrushValue;
                    LastBrushValue = BrushValue;
                    BrushSize += BrushDelta * 1.0f;
                    BrushSize = Math.Max(BrushSize, 0);

                    DrawImage(ref EditedImage, ref TempImage);
                    DrawLine(TempImage, MousePosition, MousePosition + new Vector2(1, 1), Color.FromArgb((int)(0.5f * 255.0f), Owner.CurrentSwatch.BackColor));
                }
                else
                {
                    DrawImage(ref EditedImage, ref TempImage);
                    DrawLine(TempImage, MousePosition, MousePosition + new Vector2(1, 1), Color.FromArgb((int)(0.5f * 255.0f), Owner.CurrentSwatch.BackColor));
                }
            }
            else if (State == EditorState.Text)
            {
                DrawImage(ref EditedImage, ref TempImage);
                DrawText(TempImage, Owner.TextFieldInput.Text, MousePosition, 1.0f);
                if (RMBIsDown && LastRMBIsDown)
                {
                    float BrushValue = (Position.X - MouseDownLocation.X) + (Position.Y - MouseDownLocation.Y);
                    BrushDelta = BrushValue - LastBrushValue;
                    LastBrushValue = BrushValue;

                    TextSize += BrushDelta * 0.1f;
                    TextSize = Math.Max(TextSize, 1);
                }
            }
            else if (State == EditorState.Box)
            {
                if (LMBIsDown && LastLMBIsDown)
                {
                    DrawImage(ref EditedImage, ref TempImage);
                    DrawBox(TempImage, ItemStartPosition, MousePosition);
                }
            }
            else if (State == EditorState.Arrow)
            {
                if (LMBIsDown && LastLMBIsDown)
                {
                    DrawImage(ref EditedImage, ref TempImage);
                    DrawArrow(TempImage, ItemStartPosition, MousePosition);
                }
            }
            else if (State == EditorState.Picking)
            {
                Owner.CurrentSwatch.BackColor = GetPixelSafe(EditedImage, MousePosition.X, MousePosition.Y);
            }

            if (System.Windows.Input.Keyboard.IsKeyDown(System.Windows.Input.Key.LeftAlt) && LMBIsDown)
            {
                var NewColor = GetPixelSafe(EditedImage, MousePosition.X, MousePosition.Y);
                Owner.CurrentSwatch.BackColor = NewColor;
                Clipboard.SetText($"#{NewColor.R.ToString("X2")}{NewColor.G.ToString("X2")}{NewColor.B.ToString("X2")}");
            }
            GammaCorrectedBrushSize = (BrushSize * BrushSize) * 0.01f;
            GammaCorrectedTextSize = (TextSize * TextSize) * 0.01f;
            Owner.ImageContainer.Refresh();

            //LastMousePosition = MousePosition;

        }

        public Color GetPixelSafe(Image b, float x, float y)
        {
            int X = Math.Min(Math.Max((int)x, 0), b.Width-1);
            int Y = Math.Min(Math.Max((int)y, 0), b.Height-1);
            return ((Bitmap)b).GetPixel(X, Y);
        }

        private void DrawImage(ref Image Input, ref Image TargetImage)
        {
            if(TargetImage == null)
            {
                TargetImage = new Bitmap(Input.Width, Input.Height);
            }
            using (Graphics g = Graphics.FromImage(TargetImage))
            {
                g.DrawImage(Input, new Rectangle(0, 0, Input.Width, Input.Height));
            }
        }

        private void DrawArrow(Image TargetImage, Vector2 P1, Vector2 P2)
        {
            float Distance = 10.0f;

            Vector2 Delta = Vector2.Normalize(Vector2.FromAtoB(P1, P2)) * Distance;

            Vector2 Left = Vector2.OrthagonalLeft(Delta);
            Vector2 Right = Vector2.OrthagonalRight(Delta);

            Vector2 ArrowMidpoint = Vector2.MoveTowards(P2, P1, Distance*2);

            DrawLine(TargetImage, P1, P2);
            DrawLine(TargetImage, P2, ArrowMidpoint + Left);
            DrawLine(TargetImage, P2, ArrowMidpoint + Right);
        }

        private void DrawBox(Image TargetImage, Vector2 P1, Vector2 P2)
        {
            DrawLine(TargetImage, P1, new Vector2(P1.X, P2.Y), Owner.CurrentSwatch.BackColor);
            DrawLine(TargetImage, P1, new Vector2(P2.X, P1.Y), Owner.CurrentSwatch.BackColor);
            DrawLine(TargetImage, P2, new Vector2(P1.X, P2.Y), Owner.CurrentSwatch.BackColor);
            DrawLine(TargetImage, P2, new Vector2(P2.X, P1.Y), Owner.CurrentSwatch.BackColor);
        }

        private void DrawText(Image TargetImage, string text, Vector2 Location, float opacity = 1.0f)
        {
            using (Graphics g = Graphics.FromImage(TargetImage))
            {
                g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
                Size tsize = TextRenderer.MeasureText(text, new Font(Preferences.ImageEditorFont, GammaCorrectedTextSize, (FontStyle)Preferences.FontStyleType));
                SolidBrush b = new SolidBrush(Color.FromArgb((int)(opacity * 255.0f), Owner.CurrentSwatch.BackColor));
                g.DrawString(text, new Font(Preferences.ImageEditorFont, GammaCorrectedTextSize, (FontStyle)Preferences.FontStyleType), b, new Point((int)Location.X, (int)Location.Y - tsize.Height));
            }
        }

        private void DrawLine(Image TargetImage, Vector2 P1, Vector2 P2, Color color = default(Color))
        {
            if(color == default(Color))
            {
                color = Owner.CurrentSwatch.BackColor;
            }
            using (Graphics g = Graphics.FromImage(TargetImage))
            {
                Pen MyPen = new Pen(color)
                {
                    Width = GammaCorrectedBrushSize,
                    EndCap = System.Drawing.Drawing2D.LineCap.Round,
                    StartCap = System.Drawing.Drawing2D.LineCap.Round
                };
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                g.DrawLine(MyPen, P1.ToPoint(), P2.ToPoint());
            }
        }

        public void LMBUp(Vector2 Position)
        {
            Image wat = null;
            DrawImage(ref EditedImage, ref wat);
            UndoHistory.Add((Bitmap)wat);
            if (State == EditorState.Drawing)
            {
                DrawImage(ref TempImage, ref EditedImage);
            }
            
            LMBIsDown = false;
            if (State == EditorState.Box)
            {
                DrawBox(EditedImage, ItemStartPosition, Position);
            }
            else if (State == EditorState.Arrow)
            {
                DrawArrow(EditedImage, ItemStartPosition, Position);
            }
            else if (State == EditorState.Text)
            {
                DrawText(EditedImage, Owner.TextFieldInput.Text, Position);
                Owner.FrontSwatch.Focus();
            }
            else if (State == EditorState.Picking)
            {
                Owner.ImageContainer.Cursor = Cursors.Default;
                Owner.CurrentSwatch.BackColor = GetPixelSafe(EditedImage, Position.X, Position.Y);
            }

            DrawImage(ref EditedImage, ref TempImage);
            SetState(new Vector2(0, 0), EditorState.Drawing);
            Owner.ImageContainer.Refresh();
        }

        public void RMBUp()
        {
            RMBIsDown = false;
            if (State == EditorState.Drawing)
            {
                DrawImage(ref EditedImage, ref TempImage);
            }
        }

        public void Undo()
        {
            if(UndoHistory.Count == 1)
                return;

            Image mm = (Image)UndoHistory[UndoHistory.Count - 1];
            UndoHistory.RemoveAt(UndoHistory.Count - 1);
            DrawImage(ref mm, ref EditedImage);
            DrawImage(ref EditedImage, ref TempImage);
            Owner.ImageContainer.Refresh();
        }
    }
}
