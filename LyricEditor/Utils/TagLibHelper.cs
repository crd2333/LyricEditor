using System.IO;
using System.Windows.Media.Imaging;

namespace LyricEditor.Utils
{
    public static class TagLibHelper
    {
        /// <summary>
        /// 获取音乐文件的封面图
        /// </summary>
        public static BitmapImage GetAlbumArt(string filename)
        {
            try
            {
                var file = TagLib.File.Create(filename);
                if (file.Tag.Pictures.Length > 0)
                {
                    var bin = file.Tag.Pictures[0].Data.Data;
                    BitmapImage image = new BitmapImage();
                    image.BeginInit();
                    image.StreamSource = new MemoryStream(bin);
                    image.EndInit();
                    return image;
                }
            }
            catch { } // 忽略所有TagLib可能抛出的异常
            
            return null;
        }

        /// <summary>
        /// 获取音乐文件的歌曲标题
        /// </summary>
        public static string GetTitle(string filename)
        {
            var file = TagLib.File.Create(filename);
            return file.Tag.Title;
        }
    }
}
