/*****************************************************************
 * 版权所有 (C) Lave.Zhang@outlook.com 2012
 * 本源代码仅供学习研究之用，不得用于商业目的。
 ****************************************************************/
using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;

namespace MetroGraphApp
{
    /// <summary>
    /// 地铁站点类。例如：南京西路、世纪公园。
    /// </summary>
    public class MetroNode
    {
        #region 字段区域

        private string m_name = string.Empty;
        private int m_x;
        private int m_y;
        private List<MetroLink> m_links = new List<MetroLink>();

        #endregion

        #region 属性区域

        /// <summary>
        /// 获取或设置站点名称。
        /// </summary>
        public string Name
        {
            get { return m_name; }
            set { m_name = value; }
        }

        /// <summary>
        /// 获取或设置站点的X坐标。
        /// </summary>
        public int X
        {
            get { return m_x; }
            set { m_x = value; }
        }

        /// <summary>
        /// 获取或设置站点的Y坐标。
        /// </summary>
        public int Y
        {
            get { return m_y; }
            set { m_y = value; }
        }

        /// <summary>
        /// 获取从该站点出发的路径集合。该属性始终不为空引用。
        /// </summary>
        public List<MetroLink> Links
        {
            get { return m_links; }
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
    /// 地铁站点集合类。
    /// </summary>
    public class MetroNodeCollection : KeyedCollection<string, MetroNode>
    {
        #region 方法区域

        /// <summary>
        /// 获取集合元素的唯一键。
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        protected override string GetKeyForItem(MetroNode item)
        {
            return item.Name;
        }

        #endregion
    }
}