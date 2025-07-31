using System;
using System.Text.RegularExpressions;

namespace LyricEditor.Lyric
{
    public static class LrcHelper
    {
        /// <summary>
        /// 将时间戳字符串解析为 TimeSpan
        /// </summary>
        public static TimeSpan ParseTimeSpan(string s)
        {
            // 如果毫秒是两位的，则在结尾额外补一个 0
            if (s.Split('.')[1].Length == 2)
                s += '0';
            return TimeSpan.Parse("00:" + s);
        }

        /// <summary>
        /// 尝试将时间戳字符串解析为 TimeSpan，详见 <seealso cref="ParseTimeSpan(string)"/>
        /// </summary>
        public static bool TryParseTimeSpan(string s, out TimeSpan ts)
        {
            try
            {
                ts = ParseTimeSpan(s);
                return true;
            }
            catch
            {
                ts = TimeSpan.Zero;
                return false;
            }
        }

        /// <summary>
        /// 将时间戳变为两位毫秒的格式
        /// </summary>
        /// <param name="ts"></param>
        /// <param name="isShort"></param>
        /// <returns></returns>
        public static string ToShortString(this TimeSpan ts, bool isShort = false)
        {
            if (isShort)
            {
                return $"{ts.Minutes:00}:{ts.Seconds:00}.{ts.Milliseconds / 10:00}";
            }
            else
                return $"{ts.Minutes:00}:{ts.Seconds:00}.{ts.Milliseconds:000}";
        }

        /// <summary>
        /// 平移歌词文本中 <> 包含的时间
        /// </summary>
        public static string ShiftInlineTimestamps(string text, TimeSpan offset)
        {
            var regex = new Regex(@"<(\d{1,2}:\d{2}\.\d{2,3})>");
            return regex.Replace(text, match =>
            {
                var timeStr = match.Groups[1].Value;
                if (TryParseTimeSpan(timeStr, out TimeSpan originalTime))
                {
                    var newTime = originalTime.Add(offset);
                    if (newTime < TimeSpan.Zero)
                    {
                        newTime = TimeSpan.Zero;
                    }

                    // 自动检测并保持原始精度
                    var parts = timeStr.Split('.');
                    bool isShortFormat = (parts.Length == 2 && parts[1].Length == 2);

                    return $"<{newTime.ToShortString(isShortFormat)}>";
                }
                return match.Value;
            });
        }
    }
}
