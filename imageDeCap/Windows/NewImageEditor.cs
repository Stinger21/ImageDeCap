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

namespace imageDeCap
{
    public partial class NewImageEditor : Form
    {
        public enum EditorResult
        {
            Quit,
            Upload,
            Clipboard,
            Save,
        }
        EditorResult result = EditorResult.Quit;

        public (EditorResult output, byte[] Data) FinalFunction()
        {
            byte[] outputData;
            
            if (result == EditorResult.Quit)
            {
                outputData = completeCover.GetBytes(editor.UnEditedImage, ImageFormat.Png);
            }
            else
            {
                outputData = completeCover.GetBytes(editor.EditedImage, ImageFormat.Png);
                Clipboard.Clear();
            }


            return (result, outputData);
        }
        
        PictureEditor editor;
        
        public NewImageEditor(byte[] ImageData, int X, int Y)//on start
        {
            InitializeComponent();

            System.Threading.Thread.Sleep(100);

            editor = new PictureEditor(Image.FromStream(new MemoryStream(ImageData)), this);

            int width;
            int height;
            if (editor.EditedImage.Width < 600)
                width = 600;
            else
                width = editor.EditedImage.Width;
            if (editor.EditedImage.Height < 200)
                height = 200;
            else
                height = editor.EditedImage.Height;

            this.Size = new Size(width + 40, height + 140);
            this.StartPosition = FormStartPosition.Manual;
            this.Location = new Point(X - 7, Y - 20);

            imageContainer.Image = editor.TempImage;
            
            if (Preferences.NeverUpload)
            {
                this.AcceptButton = ClipboardButton;
                this.ActiveControl = ClipboardButton;
                UploadButton.Enabled = false;
            }
            else
            {
                this.AcceptButton = UploadButton;
                this.ActiveControl = UploadButton;
            }
            CurrentButton = FrontSwatch;
        }

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


        public Button CurrentButton;

        // Mouse down
        private void imageContainer_MouseDown(object sender, MouseEventArgs e)
        {
            editor.Alt = false;
            editor.Ctrl = false;
            editor.Shift = false;
            if (System.Windows.Input.Keyboard.IsKeyDown(System.Windows.Input.Key.LeftAlt))
                editor.Alt = true;
            if (System.Windows.Input.Keyboard.IsKeyDown(System.Windows.Input.Key.LeftCtrl))
                editor.Ctrl = true;
            if (System.Windows.Input.Keyboard.IsKeyDown(System.Windows.Input.Key.LeftShift))
                editor.Shift = true;
            Point mousePos = imageContainer.PointToClient(Cursor.Position);
            if (e.Button == MouseButtons.Left)
            {
                editor.LMBDown(new Vector2(mousePos));
            }
            else if(e.Button == MouseButtons.Right)
            {
                editor.RMBDown(new Vector2(mousePos));
            }
        }
        // Mouse up
        private void imageContainer_MouseClick(object sender, MouseEventArgs e)
        {
            editor.Alt = false;
            editor.Ctrl = false;
            editor.Shift = false;
            if (System.Windows.Input.Keyboard.IsKeyDown(System.Windows.Input.Key.LeftAlt))
                editor.Alt = true;
            if (System.Windows.Input.Keyboard.IsKeyDown(System.Windows.Input.Key.LeftCtrl))
                editor.Ctrl = true;
            if (System.Windows.Input.Keyboard.IsKeyDown(System.Windows.Input.Key.LeftShift))
                editor.Shift = true;
            Point mousePos = imageContainer.PointToClient(Cursor.Position);
            if (e.Button == MouseButtons.Left)
            {
                editor.LMBUp(new Vector2(mousePos));
            }
            else if (e.Button == MouseButtons.Right)
            {
                editor.RMBUp(new Vector2(mousePos));
            }
        }
        // Mouse move
        private void imageContainer_MouseMove(object sender, MouseEventArgs e)
        {
            editor.Alt = false;
            editor.Ctrl = false;
            editor.Shift = false;
            if (System.Windows.Input.Keyboard.IsKeyDown(System.Windows.Input.Key.LeftAlt))
                editor.Alt = true;
            if (System.Windows.Input.Keyboard.IsKeyDown(System.Windows.Input.Key.LeftCtrl))
                editor.Ctrl = true;
            if (System.Windows.Input.Keyboard.IsKeyDown(System.Windows.Input.Key.LeftShift))
                editor.Shift = true;
            editor.LMBIsDown = false;
            editor.RMBIsDown = false;
            if (e.Button == MouseButtons.Left)
            {
                editor.LMBIsDown = true;
            }
            else if (e.Button == MouseButtons.Right)
            {
                editor.RMBIsDown = true;
            }
            Point mousePos = imageContainer.PointToClient(Cursor.Position);
            editor.MouseMove(new Vector2(mousePos));
        }
        
        private void Undo_Click(object sender, EventArgs e)
        {
            editor.Undo();
        }
        
        private void imageEditor_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode.ToString() == "Escape")
            {
                this.Close();
            }
            else if (e.Control)
            {
                if (e.KeyCode.ToString() == "Z")
                {
                    editor.Undo();
                }
                else if (e.KeyCode.ToString() == "C")
                {
                    Clipboard.SetImage(editor.EditedImage);
                }
            }
            if (!TextFieldInput.ContainsFocus)
            {
                if (e.KeyCode.ToString() == "T")
                {
                    addTextButton_Click(null, null);
                }
                else if (e.KeyCode.ToString() == "A")
                {
                    editor.SetState(new Vector2(imageContainer.PointToClient(Cursor.Position)), PictureEditor.EditorState.Arrow);
                }
                else if (e.KeyCode.ToString() == "B")
                {
                    editor.SetState(new Vector2(imageContainer.PointToClient(Cursor.Position)), PictureEditor.EditorState.Box);
                }
            }
            if (e.KeyCode.ToString() == "X")
            {
                Point mousePos = imageContainer.PointToClient(Cursor.Position);
                if(CurrentButton == BackSwatch)
                {
                    CurrentButton = FrontSwatch;
                }
                else if(CurrentButton == FrontSwatch)
                {
                    CurrentButton = BackSwatch;
                }
                else
                {
                    CurrentButton = FrontSwatch;
                }
                CurrentButton.BringToFront();
            }
        }

        private void currentColor_Click(object sender, EventArgs e)
        {
            if((sender as Button) == CurrentButton)
            {
                DialogResult r = colorDialog1.ShowDialog();
                if(r == DialogResult.OK)
                {
                    (sender as Button).BackColor = colorDialog1.Color;
                    CurrentButton = (Button)sender;
                }
            }
            else
            {
                CurrentButton = (Button)sender;
                (sender as Button).BringToFront();
            }
        }
        
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            editor.DrawTempText();
            imageContainer.Refresh();
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
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

        
        private void addTextButton_Click(object sender, EventArgs e)
        {
            TextFieldInput.Focus();
            TextFieldInput.SelectAll();
            editor.SetState(new Vector2(imageContainer.PointToClient(Cursor.Position)), PictureEditor.EditorState.Text);
        }
        private void PickColorButton_click(object sender, EventArgs e)
        {
            imageContainer.Cursor = Cursors.Cross;
            editor.SetState(new Vector2(imageContainer.PointToClient(Cursor.Position)), PictureEditor.EditorState.Picking);
        }
        private void AddArrowButton_Click(object sender, EventArgs e)
        {
            editor.SetState(new Vector2(imageContainer.PointToClient(Cursor.Position)), PictureEditor.EditorState.Arrow);
        }
        private void AddBoxButton_Click(object sender, EventArgs e)
        {
            editor.SetState(new Vector2(imageContainer.PointToClient(Cursor.Position)), PictureEditor.EditorState.Box);
        }
        private void HelpButton_Click(object sender, EventArgs e)
        {
            InfoText.Visible = !InfoText.Visible;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            editor.SetState(new Vector2(imageContainer.PointToClient(Cursor.Position)), PictureEditor.EditorState.Drawing);
        }

        private void imageEditor_Load(object sender, EventArgs e) { }
        
    }

    // Split the actual drawing of tihngs out into this class here because it was getting sphagettied with the UI & outputting code
    public class PictureEditor
    {
        private EditorState State = EditorState.Drawing;
        List<Bitmap> undoHistory = new List<Bitmap>();
        public Image EditedImage;
        public Image TempImage;
        public Image UnEditedImage;
        NewImageEditor owner;

        public PictureEditor(Image InputImage, NewImageEditor owner)
        {
            //Console.WriteLine("Initialize Editor");
            this.owner = owner;
            DrawImage(ref InputImage, ref UnEditedImage);
            DrawImage(ref InputImage, ref TempImage);
            DrawImage(ref InputImage, ref EditedImage);

            Image wat = null;
            DrawImage(ref InputImage, ref wat);
            undoHistory.Add((Bitmap)wat);
            SetState(new Vector2(0, 0), EditorState.Drawing);
        }

        public bool Ctrl;
        public bool Shift;
        public bool Alt;
        public bool RMBIsDown;
        public bool LMBIsDown;

        public enum EditorState
        {
            Drawing,
            Text,
            Picking,
            Box,
            Arrow,
        }

        public void SetState(Vector2 Position, EditorState State)
        {
            ItemStartPosition = Position;
            this.State = State;

            owner.BrushButton.Enabled   = true;
            owner.TextButton.Enabled    = true;
            owner.TextButton.Enabled    = true;
            owner.BoxButton.Enabled     = true;
            owner.ArrowButton.Enabled   = true;
            switch (State)
            {
                case EditorState.Drawing:
                    owner.BrushButton.Enabled = false;
                    break;
                case EditorState.Text:
                    owner.TextButton.Enabled = false;
                    break;
                case EditorState.Picking:
                    owner.TextButton.Enabled = false;
                    break;
                case EditorState.Box:
                    owner.BoxButton.Enabled = false;
                    break;
                case EditorState.Arrow:
                    owner.ArrowButton.Enabled = false;
                    break;
                default:
                    break;
            }
        }
        public void LMBDown(Vector2 Position)
        {
            LMBIsDown = true;

            if (Shift)
            {
                DrawLine(EditedImage, LastBrushLocation, Position);
            }
            BrushLocation = Position;
            LastBrushLocation = Position;
            MousePosition = Position;
            LastMousePosition = Position;

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

        public float BrushSize = 20.0f;
        public float TextSize = 35.0f;
        public float GammaCorrectedBrushSize = 20.0f;
        public float GammaCorrectedTextSize = 35.0f;

        Vector2 ItemStartPosition;

        Vector2 BrushLocation;
        Vector2 LastBrushLocation;
        Vector2 MousePosition;
        Vector2 LastMousePosition;

        Vector2 MouseDownLocation;

        float BrushDelta = 0;
        float LastBrushValue = 0;

        public void DrawTempText()
        {
            DrawImage(ref EditedImage, ref TempImage);
            DrawText(TempImage, owner.TextFieldInput.Text, MousePosition);
        }

        public void MouseMove(Vector2 Position)
        {
            if (!RMBIsDown)
            {
                MousePosition = Position;
            }
            if (State == EditorState.Drawing)
            {
                if (LMBIsDown)
                {
                    float Distance = Vector2.Distance(BrushLocation, MousePosition);
                    if (Distance > Preferences.BrushSmoothingDistance)
                    {
                        BrushLocation = Vector2.MoveTowards(BrushLocation, MousePosition, Distance - Preferences.BrushSmoothingDistance);
                        DrawLine(TempImage, LastBrushLocation, BrushLocation, owner.CurrentButton.BackColor, GammaCorrectedBrushSize);
                    }
                    LastBrushLocation = BrushLocation;
                }
                else if (RMBIsDown)
                {
                    float BrushValue = (Position.X - MouseDownLocation.X) + (Position.Y - MouseDownLocation.Y);
                    BrushDelta = BrushValue - LastBrushValue;
                    LastBrushValue = BrushValue;
                    BrushSize += BrushDelta * 1.0f;
                    BrushSize = Math.Max(BrushSize, 0);

                    DrawImage(ref EditedImage, ref TempImage);
                    DrawLine(TempImage, MousePosition, MousePosition + new Vector2(1, 1), Color.FromArgb((int)(0.5f * 255.0f), owner.CurrentButton.BackColor), GammaCorrectedBrushSize);
                }
                else
                {
                    DrawImage(ref EditedImage, ref TempImage);
                    DrawLine(TempImage, MousePosition, MousePosition + new Vector2(1, 1), Color.FromArgb((int)(0.5f * 255.0f), owner.CurrentButton.BackColor), GammaCorrectedBrushSize);
                }
            }
            else if (State == EditorState.Text)
            {
                DrawImage(ref EditedImage, ref TempImage);
                DrawText(TempImage, owner.TextFieldInput.Text, MousePosition, 1.0f);
                if (RMBIsDown)
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
                if (LMBIsDown)
                {
                    DrawImage(ref EditedImage, ref TempImage);
                    DrawBox(TempImage, ItemStartPosition, MousePosition);
                }
            }
            else if (State == EditorState.Arrow)
            {
                if (LMBIsDown)
                {
                    DrawImage(ref EditedImage, ref TempImage);
                    DrawArrow(TempImage, ItemStartPosition, MousePosition);
                }
            }
            else if (State == EditorState.Picking)
            {
                owner.CurrentButton.BackColor = ((Bitmap)EditedImage).GetPixel((int)MousePosition.X, (int)MousePosition.Y);
            }

            if (System.Windows.Input.Keyboard.IsKeyDown(System.Windows.Input.Key.LeftAlt))
            {
                var NewColor = ((Bitmap)EditedImage).GetPixel((int)MousePosition.X, (int)MousePosition.Y);
                owner.CurrentButton.BackColor = NewColor;
                Clipboard.SetText("#" + NewColor.R.ToString("X2") + NewColor.G.ToString("X2") + NewColor.B.ToString("X2"));
            }
            GammaCorrectedBrushSize = (BrushSize * BrushSize) * 0.01f;
            GammaCorrectedTextSize = (TextSize * TextSize) * 0.01f;
            LastMousePosition = MousePosition;
            owner.imageContainer.Refresh();
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

            Vector2 Delta = Vector2.Normalize(Vector2.fromAtoB(P1, P2)) * Distance;

            Vector2 Left = Vector2.OrthagonalLeft(Delta);
            Vector2 Right = Vector2.OrthagonalRight(Delta);

            Vector2 ArrowMidpoint = Vector2.MoveTowards(P2, P1, Distance*2);

            DrawLine(TargetImage, P1, P2);
            DrawLine(TargetImage, P2, ArrowMidpoint + Left);
            DrawLine(TargetImage, P2, ArrowMidpoint + Right);
        }

        private void DrawBox(Image TargetImage, Vector2 P1, Vector2 P2)
        {
            DrawLine(TargetImage, P1, new Vector2(P1.X, P2.Y), owner.CurrentButton.BackColor, 3);
            DrawLine(TargetImage, P1, new Vector2(P2.X, P1.Y), owner.CurrentButton.BackColor, 3);
            DrawLine(TargetImage, P2, new Vector2(P1.X, P2.Y), owner.CurrentButton.BackColor, 3);
            DrawLine(TargetImage, P2, new Vector2(P2.X, P1.Y), owner.CurrentButton.BackColor, 3);
        }

        private void DrawText(Image TargetImage, string text, Vector2 Location, float opacity = 1.0f)
        {
            using (Graphics g = Graphics.FromImage(TargetImage))
            {
                Pen MyPen = new Pen(owner.CurrentButton.BackColor);
                MyPen.Width = GammaCorrectedTextSize;
                g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
                Size tsize = TextRenderer.MeasureText(text, new Font(Preferences.ImageEditorFont, GammaCorrectedTextSize, (FontStyle)Preferences.FontStyleType));
                SolidBrush b = new SolidBrush(Color.FromArgb((int)(opacity * 255.0f), owner.CurrentButton.BackColor));
                g.DrawString(text, new Font(Preferences.ImageEditorFont, GammaCorrectedTextSize, (FontStyle)Preferences.FontStyleType), b, new Point((int)Location.X, (int)Location.Y - tsize.Height));
            }
        }

        private void DrawLine(Image TargetImage, Vector2 P1, Vector2 P2, Color color = default(Color), float size = 4)
        {
            if(color == default(Color))
            {
                color = owner.CurrentButton.BackColor;
            }
            using (Graphics g = Graphics.FromImage(TargetImage))
            {
                Pen MyPen = new Pen(color);
                MyPen.Width = size;
                MyPen.EndCap = System.Drawing.Drawing2D.LineCap.Round;
                MyPen.StartCap = System.Drawing.Drawing2D.LineCap.Round;
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                g.DrawLine(MyPen, P1.ToPoint(), P2.ToPoint());
            }
        }

        public void LMBUp(Vector2 Position)
        {

            Image wat = null;
            DrawImage(ref EditedImage, ref wat);
            undoHistory.Add((Bitmap)wat);
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
                DrawText(EditedImage, owner.TextFieldInput.Text, Position);
                owner.FrontSwatch.Focus();
            }
            else if (State == EditorState.Picking)
            {
                owner.imageContainer.Cursor = Cursors.Default;
                owner.CurrentButton.BackColor =  ((Bitmap)EditedImage).GetPixel((int)Position.X, (int)Position.Y);
            }

            DrawImage(ref EditedImage, ref TempImage);
            SetState(new Vector2(0, 0), EditorState.Drawing);
            owner.imageContainer.Refresh();
        }

        public void RMBUp(Vector2 Position)
        {
            RMBIsDown = false;
            if (State == EditorState.Drawing)
            {
                DrawImage(ref EditedImage, ref TempImage);
            }
        }

        public void Undo()
        {
            if(undoHistory.Count == 1)
                return;
            Image mm = (Image)undoHistory[undoHistory.Count - 1];
            undoHistory.RemoveAt(undoHistory.Count - 1);
            DrawImage(ref mm, ref EditedImage);
            DrawImage(ref EditedImage, ref TempImage);
            owner.imageContainer.Refresh();
        }
    }

    // TARGET - GOAL
    // Little vector class because Points drive me insane :)
    public struct Vector2
    {
        // Members
        public float X;
        public float Y;

        // Ctor
        public Vector2(Vector2 vector)
        {
            this.X = vector.X;
            this.Y = vector.Y;
        }
        public Vector2(Point vector)
        {
            this.X = vector.X;
            this.Y = vector.Y;
        }
        public Vector2(float X, float Y)
        {
            this.X = X;
            this.Y = Y;
        }

        // Utility
        public Point ToPoint()
        {
            return new Point((int)X, (int)Y);
        }
        // Math
        public static float Magnitude(Vector2 P1)
        {
            return (float)Math.Sqrt(P1.X * P1.X + P1.Y * P1.Y);
        }
        public static float Distance(Vector2 P1, Vector2 P2)
        {
            return Vector2.Magnitude(P2 - P1);
        }
        public static Vector2 Normalize(Vector2 P1)
        {
            float magn = Magnitude(P1);
            if (magn == 0)
            {
                return new Vector2(0, 0);
            }
            return P1 / magn;
        }
        public static Vector2 MoveTowards(Vector2 P1, Vector2 P2, float MaxDistance)
        {
            Vector2 DeltaVector = P2 - P1;
            float Distance = Magnitude(DeltaVector);
            Distance = Math.Min(Distance, MaxDistance);
            return P1 + Normalize(DeltaVector) * Distance;
        }
        public static Vector2 OrthagonalLeft(Vector2 P1)
        {
            return new Vector2(-P1.Y, P1.X);
        }
        public static Vector2 OrthagonalRight(Vector2 P1)
        {
            return new Vector2(P1.Y, -P1.X);
        }
        public static Vector2 fromAtoB(Vector2 A, Vector2 B)
        {
            return B - A;
        }
        public static Vector2 Lerp(Vector2 A, Vector2 B, float t)
        {
            return (A + (B - A) * t);
        }

        // Operators
        public static Vector2 operator +(Vector2 vector1, Vector2 vector2)
        {
            return new Vector2(vector1.X + vector2.X, vector1.Y + vector2.Y);
        }
        public static Vector2 operator -(Vector2 vector1, Vector2 vector2)
        {
            return new Vector2(vector1.X - vector2.X, vector1.Y - vector2.Y);
        }
        public static Vector2 operator *(Vector2 vector1, Vector2 vector2)
        {
            return new Vector2(vector1.X * vector2.X, vector1.Y * vector2.Y);
        }
        public static Vector2 operator /(Vector2 vector1, Vector2 vector2)
        {
            return new Vector2(vector1.X / vector2.X, vector1.Y / vector2.Y);
        }

        public static Vector2 operator +(Vector2 vector1, float vector2)
        {
            return new Vector2(vector1.X + vector2, vector1.Y + vector2);
        }
        public static Vector2 operator -(Vector2 vector1, float vector2)
        {
            return new Vector2(vector1.X - vector2, vector1.Y - vector2);
        }
        public static Vector2 operator *(Vector2 vector1, float vector2)
        {
            return new Vector2(vector1.X * vector2, vector1.Y * vector2);
        }
        public static Vector2 operator /(Vector2 vector1, float vector2)
        {
            return new Vector2(vector1.X / vector2, vector1.Y / vector2);
        }
    }

}
