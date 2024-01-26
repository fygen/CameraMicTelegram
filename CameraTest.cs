using Emgu.CV;
using Emgu.CV.CvEnum;
using System.Text.RegularExpressions;

//namespace CameraMicTelegram
//{
//    public class Camera
//    {
//        private Mat Image;
//        private string FileName { get; set; }
//        private VideoCapture Capture {  get; set; }
//        private static Camera()
//        {
//            VideoCapture Capture = new VideoCapture(0);
//            this.Capture = Capture;
//        }

//        public async Task<Camera> TakeSavePic(string FileName = "pict")
//        {
//            return await SavePic(await TakePic(), FileName);
//        }
//        public static async Task<Camera> TakePic()
//        {
//            Camera camera = new Camera();
//            try
//            {
//                Capture.Set(CapProp.FrameWidth, 1280);
//                Capture.Set(CapProp.FrameHeight, 720);
//                camera.Image = capture.QueryFrame();
//                capture.Dispose();
//            }
//            catch (Exception)
//            {
//                camera.Image = new Mat();
//            }
//            return camera;
//        }
//        public async Task<Camera> SavePic(Mat image, string FileName)
//        {
//            try
//            {
//                image.Save(Environment.CurrentDirectory + "/assets/" + FileName + ".jpg");
//                image.Dispose();
//            }
//            catch (Exception)
//            {
//                throw new Exception();
//            }
//            return this;
//        }
//    }
//}


namespace CameraMicTelegram
{
    public class CameraTest
    {
        private Mat Image { get; set; }
        private string FileName { get; set; }

        private CameraTest() { }

        public static CameraTest TakePic()
        {
            Camera camera = new Camera();
            try
            {
                VideoCapture capture = new VideoCapture(0);
                capture.Set(CapProp.FrameWidth, 1280);
                capture.Set(CapProp.FrameHeight, 720);
                camera.Image = capture.QueryFrame();
                capture.Dispose();
            }
            catch (Exception)
            {
                camera.Image = new Mat();
            }
            return camera;
        }

        public CameraTest SavePic(string fileName = "pict")
        {
            FileName = fileName;
            try
            {
                Image.Save(Environment.CurrentDirectory + "/assets/" + FileName + ".jpg");
                Image.Dispose();
            }
            catch (Exception)
            {
                // Handle exceptions appropriately
            }
            return this;
        }

        public static async Task<Boolean> TakeSavePic(string fileName = "pict")
        {
            return await Task.Run(() => TakePic().SavePic(fileName).FileName != null);
        }
    }
}