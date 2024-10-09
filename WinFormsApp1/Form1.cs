using DynamicTesseract;
using IronOcr;
using OpenCvSharp;
using System.Threading.Tasks;
using Tesseract;
using CvConverter = OpenCvSharp.Extensions.BitmapConverter;
using CvPoint = OpenCvSharp.Point;
using Rect = OpenCvSharp.Rect;

namespace WinFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        //���� ����� ȭ�� �޾ƿ��� �ڵ�
        //test1.jpg, test2.jpg�� ȭ���� ����ȭ���� �ƴ϶� �߾ȵ�;;;;
        Mat getMobileScreenShot()
        {
            return new Mat(@"test3.jpg");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //����
            Mat origin = getMobileScreenShot();
            //���� �����°� ¥���� 
            Rect test1Rect = new Rect(0, 400, origin.Width, origin.Height - 650);
            Rect test2Rect = new Rect(0, 600, origin.Width, origin.Height - 1100);

            Mat crop = origin.SubMat(test2Rect);
            Mat binary = crop.Threshold(249, 0, ThresholdTypes.Tozero);

            //���� �׵θ� �κ� rgb�� ���ϱ�
            //Vec3b pc = origin.At<Vec3b>(225, 430);
            //var b = pc.Item0;
            //var g = pc.Item1;
            //var r = pc.Item2;

            //�׵θ� ������ ���� ������
            CvPoint[][] point;
            HierarchyIndex[] hier;
            Scalar borderColor = new Scalar(250,250,250);
            Scalar borderColor2 = new Scalar(255,255,255);

            //Ư�� ���� ������ �����
            Mat inrange = crop.InRange(borderColor, borderColor2);
            
            //�׵θ� �����Լ�
            Cv2.FindContours(inrange, out point, out hier, RetrievalModes.Tree, ContourApproximationModes.ApproxTC89L1);
            List<CvPoint[]> realPoint = new List<CvPoint[]>();
            int test1L = 290;
            int test2L = 600;
            foreach (CvPoint[] p in point)
            {
                double length = Cv2.ArcLength(p, true);
                if (length > test2L) //���� ����Ʈ�� ���� ���ѵּ� �ٽ� ����Ʈ ���� 
                {
                    realPoint.Add(p);
                }
            }

            // �׵θ� ���� �� ����Ʈ �̾� �׵θ� �׷����� 
            Cv2.DrawContours(crop, realPoint, -1, new Scalar(100, 0, 0), 3, LineTypes.AntiAlias, null, 1);

            List<Rect> rectList = new List<Rect>();
            foreach (CvPoint[] p in realPoint)
            {
                int m = 30;
                Rect r = Cv2.BoundingRect(p);
                Rect smallRect = new Rect(r.X + m, r.Y + m, r.Width - 2 * m, r.Height - 2 * m);
                rectList.Add(smallRect);
                Cv2.Rectangle(crop, r, new Scalar(0, 200, 0), 2);
            }
            rectList = rectList.OrderBy(r => r.Y).ThenBy(r => r.X).ToList();

            // �׵θ� ����� ����Ʈ�� �߾Ӱ����ϱ� �ʿ���µ� �����; �� �غ�
            foreach (CvPoint[] p in realPoint)
            {
                Moments m = Cv2.Moments(p);
                int x = (int)(m.M10 / m.M00);
                int y = (int)(m.M01 / m.M00);
                Cv2.Circle(crop, new CvPoint(x, y), 5, new Scalar(0, 0, 200), -1);
            }

            

            List<Mat> matList = new List<Mat>();
            foreach(var r in rectList)
            {
                matList.Add(binary.SubMat(r).MedianBlur(3));
            }

            // �ȼ��� ���°ɷ� �ٸ��� �Ǻ��ϴ°� �Ұ���
            //List<int> blackCountList = new List<int>();


            /*
            foreach (Mat m in matList)
            {
                int blackCount = 0;
                for (int y = 0; y < m.Cols; y++)
                {
                    for (int x = 0; x < m.Rows; x++)
                    {
                        if (m.At<byte>(x, y) == 0)
                        {
                            blackCount++;
                        }
                    }
                }
                blackCountList.Add(blackCount);
            }
            
            */


            /*
            foreach (Mat m in matList)
            {
                Bitmap b = CvConverter.ToBitmap(m);

                int blackCount = 0;

                for (int x = 0; x < b.Width; x++)
                {
                    for (int y = 0; y < b.Height; y++)
                    {
                        if (b.GetPixel(x, y) == Color.FromArgb(255,0,0,0))
                        {
                            blackCount++;
                        }
                    }
                }

                blackCountList.Add(blackCount);
            }
            
            */


            //////////////////////OCR �� ��ü //////////////////////////////

            List<string> textList = new List<string>();
            foreach (Mat m in matList)
            {
                using (var ocr = new TesseractEngine("./tessdata", "kor", Tesseract.EngineMode.Default))
                {
                    using (Bitmap b = CvConverter.ToBitmap(m))
                    {
                        using (var data = ocr.Process(b))
                        {
                            //MessageBox.Show(data.GetText());
                            textList.Add(data.GetText().Replace("\r", "").Replace("\n", "").Trim());
                        }
                    }
                }
            }

            string commonString = "";
            int answer = 0;

            for(int i =0; i<textList.Count; i++)
            {
                //MessageBox.Show(s);
                string text = textList[i];
                textBox2.Text += (i+1).ToString() + "�� ����:" + textList[i] + "\r\n";

                if(answer == 0)
                {
                    for (int o = 0; o < textList.Count; o++)
                    {
                        if (o == i) continue;

                        if (text == textList[o])
                        {
                            commonString = text;
                            continue;
                        }

                        if ( (commonString != "") && (text != textList[o]) )
                        {
                            answer = o;
                            break;
                        }
                    }
                }
                
            }


            textBox2.Text += "\r\n" + "������:" + (answer+1).ToString();


            //var Ocr = new IronTesseract();
            //Ocr.Language = OcrLanguage.Korean;
            //foreach (Mat m in matList)
            //{
            //    using (Bitmap b = CvConverter.ToBitmap(m))
            //    {
            //        var Result = Ocr.Read(b);
            //        var text = Result.Text;
            //        MessageBox.Show(text);
            //    }
            //}


            pictureBox1.Image = CvConverter.ToBitmap(crop);

            /*
            
            string desk = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            for(int i=0; i<matList.Count; i++)
            {
                Cv2.ImWrite(System.IO.Path.Combine(desk, "m" + i.ToString() + ".jpg"), matList[i]);
            }
            */
            
        }
    }
}
