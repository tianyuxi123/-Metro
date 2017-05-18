/*****************************************************************
 * 版权所有 (C) Lave.Zhang@outlook.com 2012
 * 本源代码仅供学习研究之用，不得用于商业目的。
 ****************************************************************/
using System;

namespace MetroGraphApp
{
    /// <summary>
    /// 地铁路径类。
    /// 该类表示两个站点之间的行车路径。
    /// 路径是双向的，如果有A->B，一定有一条B->A。
    /// </summary>
    public class MetroLink
    {
        #region 构造区域

        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="line">所属线路。</param>
        /// <param name="from">来源站点。</param>
        /// <param name="to">目标站点。</param>
        /// <exception cref="ArgumentNullException">如果from或to为空引用，则抛出该异常。</exception>
        public MetroLink(MetroLine line, MetroNode from, MetroNode to)
        {
            if (line == null) throw new ArgumentNullException("line");
            if (from == null) throw new ArgumentNullException("from");
            if (to == null) throw new ArgumentNullException("to");

            m_line = line;
            m_from = from;
            m_to = to;
        }

        #endregion

        #region 字段区域

        private MetroNode m_from;
        private MetroNode m_to;
        private MetroLine m_line;
        private float m_weight;
        private int m_flag;

        #endregion

        #region 属性区域

        /// <summary>
        /// 获取所属线路。该属性始终不为空引用。
        /// </summary>
        public MetroLine Line
        {
            get { return m_line; }
        }

        /// <summary>
        /// 获取来源站点。该属性始终不为空引用。
        /// </summary>
        public MetroNode From
        {
            get { return m_from; }
        }

        /// <summary>
        /// 获取目标站点。该属性始终不为空引用。
        /// </summary>
        public MetroNode To
        {
            get { return m_to; }
        }

        /// <summary>
        /// 获取或设置权重（运行时长，单位：分钟）。
        /// </summary>
        public float Weight
        {
            get { return m_weight; }
            set { m_weight = value; }
        }

        /// <summary>
        /// 获取或设置内部标识。
        /// 0表示无并轨；1表示第一条线路；2表示第二条线路；-1表示反向路径，不与绘制。
        /// </summary>
        public int Flag
        {
            get { return m_flag; }
            set { m_flag = value; }
        }

        #endregion

        #region 方法区域

        /// <summary>
        /// 返回当前对象的字符串表示。
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("{0}:{1}->{2}", this.Line.Name, this.From.Name, this.To.Name);
        }

        #endregion
    }
}