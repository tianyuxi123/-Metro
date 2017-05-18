/*****************************************************************
 * 版权所有 (C) Lave.Zhang@outlook.com 2012
 * 本源代码仅供学习研究之用，不得用于商业目的。
 ****************************************************************/
using System;
using System.Drawing;
using System.Collections.ObjectModel;

namespace MetroGraphApp
{
    /// <summary>
    /// 地铁线路类。例如：1号线、2号线。
    /// </summary>
    public class MetroLine
    {
        #region 字段区域

        private string m_name = string.Empty;
        private Color m_color = Color.Black;

        #endregion

        #region 属性区域

        /// <summary>
        /// 获取或设置线路的名称。
        /// </summary>
        public string Name
        {
            get { return m_name; }
            set { m_name = value; }
        }

        /// <summary>
        /// 获取或设置线路的颜色。
        /// </summary>
        public Color Color
        {
            get { return m_color; }
            set { m_color = value; }
        }

        #endregion

        #region 方法区域

        /// <summary>
        /// 返回当前对象的字符串表示。
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return this.Name;
        }

        #endregion
    }

    /// <summary>
    /// 地铁线路集合类。
    /// </summary>
    public class MetroLineCollection : KeyedCollection<string, MetroLine>
    {
        #region 方法区域

        /// <summary>
        /// 获取集合元素的唯一键。
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        protected override string GetKeyForItem(MetroLine item)
        {
            return item.Name;
        }

        #endregion
    }
}