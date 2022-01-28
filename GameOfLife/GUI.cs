using GameOfLife;

namespace CS_GUI
{
    class GUI
    {
        private Form1 _Form;
        private Bitmap _Bitmap;
        private Graphics graphicsObj;
        private Pen _Pen;
        private System.Drawing.SolidBrush _Brush;
        public GUI(Form1 _Form)
        {
            this._Form = _Form;
            _Bitmap = new Bitmap(_Form.Height, _Form.Width);
            graphicsObj = Graphics.FromImage(_Bitmap);
            _Pen = new Pen(Color.Black);
            _Brush = new System.Drawing.SolidBrush(Color.White);
        }

        public void Print()
        {
            _Form.graphicsObj.DrawImage(_Bitmap, 0, 0);
        }
        public void Reset()
        {
            _Brush.Color = Color.White;
            graphicsObj.FillRectangle(_Brush, 0, 0, _Bitmap.Width, _Bitmap.Height);
            // _Bitmap = new Bitmap(_Bitmap.Width, _Bitmap.Height);
        }

        public void DrawLine(int x1, int y1, int x2, int y2, Color color, int width)
        {
            _Pen.Color = color;
            _Pen.Width = width;
            graphicsObj.DrawLine(_Pen, x1, y1, x2, y2);
        }

        public void DrawRectangle(int x1, int y1, int x2, int y2, Color color)
        {
            _Pen.Color = color;
            graphicsObj.DrawRectangle(_Pen, x1, y1, x2 - x1, y2 - y1);
        }

        public void FilledRectangle(int x1, int y1, int x2, int y2, Color color)
        {
            _Brush.Color = color;
            graphicsObj.FillRectangle(_Brush, x1, y1, x2 - x1, y2 - y1);
        }

        private void DrawLine(Point one, Point two, Color color, int width)
        {
            _Pen.Color = color;
            _Pen.Width = width;
            graphicsObj.DrawLine(_Pen, one.X, one.Y, two.X, two.Y);
        }

        public void DrawBitmap(Bitmap canvas, int x, int y)
        {
            graphicsObj.DrawImage(canvas, x, y);
        }

        public void DrawBitmap(string Path, int x, int y)
        {
            Bitmap canvas = new Bitmap(Path);
            graphicsObj.DrawImage(canvas, x, y);
        }
    }
}
/*
The way the GUI works is that it has some methods made by me that can draw onto its own bitmap
when all the things is drawn there can be called a fun
*/