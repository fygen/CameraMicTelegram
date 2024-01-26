using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.Util;
using Emgu.CV.Cuda;
using System;
using System.Drawing;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Emgu.CV.Reg;

namespace CameraMicTelegram
{
    public class CameraModule
    {
        private Mat Image { get; set; }
        private Image<Bgr, byte> BgrImage { get; set; }
        private string FileName { get; set; }
        private DateTime TakenDate { get; set; }

        private string ErrorMessage { get; set; }

        private CameraModule()
        {
            TakenDate = DateTime.Now;
        }

        public static CameraModule TakePic()
        {
            CameraModule Camera = new CameraModule();
            try
            {
                VideoCapture capture = new VideoCapture(0);
                capture.Set(CapProp.FrameWidth, 1280);
                capture.Set(CapProp.FrameHeight, 720);
                Camera.Image = capture.QueryFrame();
                Camera.BgrImage = Camera.Image.ToImage<Bgr, byte>();
                capture.Dispose();
            }
            catch (Exception)
            {
                Camera.Image = new Mat();
            }
            return Camera;
        }

        public CameraModule SavePic(string fileName)
        {
            if (fileName.Length > 0)
                FileName = fileName;
            else
                FileName = DateTime.Now.ToString();
            try
            {
                BgrImage.Save(Environment.CurrentDirectory + "/assets/" + FileName + ".jpg");
                Image.Dispose();
            }
            catch (Exception)
            {
                ErrorMessage = "Cannot save picture, might be no BgrImage?"; 
            }
            return this;
        }

        public CameraModule CheckFace()
        {
            CascadeClassifier haar = new CascadeClassifier(Environment.CurrentDirectory + "/assets/haarcascade_frontalface_default.xml");
            Image<Gray, byte> grayframe = Image.ToImage<Gray, byte>();
            var faces = haar.DetectMultiScale(
                            grayframe,
                            1.4,
                            4,
                            new Size(grayframe.Width / 8, grayframe.Height / 8),
                            Size.Empty);

            foreach (var face in faces)
            {
                this.BgrImage.Draw(face, new Bgr(0, double.MaxValue, 100), 3);

            }
            return this;
        }

        public static CameraModule WatchFaces()
        {
            CameraModule Camera = new CameraModule();
            VideoCapture capture = new VideoCapture(0);
            capture.Set(CapProp.FrameWidth, 1280);
            capture.Set(CapProp.FrameHeight, 720);
            CascadeClassifier haar = new CascadeClassifier(Environment.CurrentDirectory + "/assets/haarcascade_frontalface_default.xml");
            using (Camera.Image = capture.QueryFrame())
            {
                if (Camera.Image != null)
                {
                    Camera.BgrImage = Camera.Image.ToImage<Bgr, byte>();
                    Image<Gray, byte> grayframe = Camera.Image.ToImage<Gray, byte>();
                    var faces = haar.DetectMultiScale(
                                    grayframe,
                                    1.4,
                                    4,
                                    new Size(grayframe.Width / 8, grayframe.Height / 8),
                                    Size.Empty);
                    foreach (var face in faces)
                    {
                        Camera.BgrImage.Draw(face, new Bgr(0, double.MaxValue, 100), 3);
                    }
                    if (faces.Length != 0)
                    {
                        return Camera;
                    }
                }
                else
                {
                    Camera.ErrorMessage = "Camera image is not available.";
                    return Camera;
                }
            }
            Camera.ErrorMessage = "Cannot reach frames. Could be camera used by other software? or software problems.";
            return Camera;
        }

        public static async Task<Boolean> TakeSavePic(string fileName = "pict")
        {
            return await Task.Run(() => TakePic().SavePic(fileName).FileName != null);
        }

        public static async Task<Boolean> FaceAndSave(string fileName = "pict")
        {
            return await Task.Run(() => TakePic().CheckFace().SavePic(fileName).FileName != null);
        }

        public static async Task<Boolean> WatchAndSee(string fileName= "catch") 
        {
            return await Task.Run(() => WatchFaces().SavePic(fileName).FileName != null  );
        }
    }
}
