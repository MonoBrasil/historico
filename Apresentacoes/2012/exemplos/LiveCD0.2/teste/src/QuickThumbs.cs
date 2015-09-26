using System;
using System.IO;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

public class QuickThumbs
{
	
	private int thumbWidth;
	private int thumbHeight;

	public QuickThumbs(int thumbWidth, int thumbHeight) {
		this.thumbWidth = thumbWidth;
		this.thumbHeight = thumbHeight;
	}
	
	private readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
	
	public void GenerateThumb(string imageInPath, string thumbOutPath)
	{
		log.Info("GenerateThumb(" + imageInPath + ", " + thumbOutPath + ")");

		try {
			if (Directory.Exists(thumbOutPath.Substring(0,thumbOutPath.LastIndexOf("/"))) == false)
				Directory.CreateDirectory(thumbOutPath.Substring(0,thumbOutPath.LastIndexOf("/")));

			using (Image imageIn = System.Drawing.Image.FromFile(imageInPath)) {

				int width = thumbWidth;
				int height = thumbHeight;

				/* Is the image smaller than our thumbnail size? */

				if (imageIn.Width < thumbWidth)
					width = imageIn.Width;

				if (imageIn.Height < thumbHeight)
					height = imageIn.Height;
				
				/* Is the image tall ? */

				if (imageIn.Height > imageIn.Width) {
					height = thumbHeight;
					width = Convert.ToInt32 ((double)height * ((double)imageIn.Width / (double)imageIn.Height));
				}

				/* Is the image wide ? */

				if (imageIn.Height < imageIn.Width) {
					width = thumbWidth;
					height = Convert.ToInt32( (double)width * ((double)imageIn.Height / (double)imageIn.Width) );
				}

				/* Center the thumbnail */

				int x = (thumbWidth / 2) - (width /2);
				int y = (thumbHeight / 2) - (height /2);
			
				Console.WriteLine ("New Thumb: " + x + " " + y + " " + width + " " + height);
				Image.GetThumbnailImageAbort dummyCallback = new Image.GetThumbnailImageAbort (ThumbnailCallback);
				Image thumbImage = imageIn.GetThumbnailImage (width, height, dummyCallback, IntPtr.Zero);
				Bitmap imageOut = new Bitmap (thumbWidth, thumbHeight);
				Graphics graphicsOut = Graphics.FromImage (imageOut);
				graphicsOut.Clear (Color.Gray);
				graphicsOut.DrawImage (thumbImage, x, y);
				imageOut.Save (thumbOutPath, ImageFormat.Jpeg);
			
			}

		} catch (Exception ex) {
			log.Error("Exception in GenerateThumb!", ex);
			throw ex;
		}
	}

	private bool ThumbnailCallback () 
	{
		return false;
	}
	
	public void ResizePhoto(string imageInPath, string imageOutPath, int width) {
		log.Info("ResizePhoto(" + imageInPath + ", " + imageOutPath + ", " + width.ToString() + ")");
		try {
			Directory.CreateDirectory(imageOutPath.Substring(0,imageOutPath.LastIndexOf("/")));

			/* Will this work just as well as the above method? */
			Bitmap bitmapIn = new Bitmap(imageInPath);

			if (width > bitmapIn.Width)
				throw new Exception("You can't size an image larger than the original! Please select a different size.");

			int height = Convert.ToInt32( (double)width * ((double)bitmapIn.Height / (double)bitmapIn.Width) );

			Size newSize = new Size(width, height);
			Bitmap bitmapOut = new Bitmap(bitmapIn, newSize);
			bitmapOut.Save(imageOutPath, ImageFormat.Jpeg);

			bitmapIn.Dispose();
			bitmapOut.Dispose();

		} catch (Exception ex) {
			log.Error("Exception in ResizePhoto!", ex);
			throw ex;
		}
	}

	public static string CombinePath (string path1, string path2)
	{
		if (path2.StartsWith ("/") == true)
			path2 = path2.Substring (1);
		
		if (path1.EndsWith ("/") == false)
			return path1 + "/" + path2;
		else
			return path1 + path2;
	}

}
