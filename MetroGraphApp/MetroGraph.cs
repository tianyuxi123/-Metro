/*****************************************************************
 * 版权所有 (C) Lave.Zhang@outlook.com 2012
 * 本源代码仅供学习研究之用，不得用于商业目的。
 ****************************************************************/
using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;

namespace MetroGraphApp
{
    /// <summary>
    /// 地铁线路图类。
    /// </summary>
    public class MetroGraph
    {
        #region 字段区域

        private MetroLineCollection m_lines = new MetroLineCollection();
        private MetroNodeCollection m_nodes = new MetroNodeCollection();

        #endregion

        #region 属性区域

        /// <summary>
        /// 获取地铁线路的集合。该属性始终不为空引用。
        /// </summary>
        public MetroLineCollection Lines
        {
            get { return m_lines; }
        }

        /// <summary>
        /// 获取地铁站点的集合。该属性始终不为空引用。
        /// </summary>
        public MetroNodeCollection Nodes
        {
            get { return m_nodes; }
        }

        /// <summary>
        /// 获取地铁路径的枚举迭代。该属性始终不为空引用。
        /// </summary>
        public IEnumerable<MetroLink> Links
        {
            get
            {
                foreach (var node in this.Nodes)
                {
                    foreach (var link in node.Links)
                    {
                        yield return link;
                    }
                }
            }
        }

        #endregion

        #region 方法区域

        /// <summary>
        /// 获取指定两个线路的中转站。
        /// </summary>
        /// <param name="line1">线路1。</param>
        /// <param name="line2">线路2。</param>
        /// <returns>中转站。</returns>
        /// <exception cref="ArgumentNullException">如果line1或line2为空引用，则抛出该异常。</exception>
        public IEnumerable<MetroNode> GetTransferNodes(MetroLine line1, MetroLine line2)
        {
            if (line1 == null) throw new ArgumentNullException("line1");
            if (line2 == null) throw new ArgumentNullException("line2");
            if (line1 == line2) yield break;

            foreach (var node in this.Nodes.Where(c => c.Links.Count > 2
               && c.Links.Exists(k => k.Line == line1) && c.Links.Exists(k => k.Line == line2)))
            {
                yield return node;
            }
        }

        #endregion
    }
}