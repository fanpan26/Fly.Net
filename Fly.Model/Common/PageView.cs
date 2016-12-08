using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fly.Model.Common
{
    /// <summary>
    /// 分页结果
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PageView<T>
    {

        public PageView()
        {
            
        }
        public PageView(int pageIndex,int pageSize) : base()
        {
            _pageIndex = pageIndex > 0 ? pageIndex : 1;
            _pageSize = pageSize > 0 ? pageSize : 20;
        }

        private int _pageIndex;
        private int _pageSize;
        private int _totalPageCount = 0;
        private int _totalCount = 0;
        [JsonProperty("tcount")]
        public int TotalCount
        {
            get { return _totalCount; }
            set
            {
                _totalCount = value;
                _totalPageCount = Convert.ToInt32(Math.Ceiling(TotalCount * 1.0 / PageSize));
                _pageIndex = _pageIndex > _totalPageCount ? _totalPageCount : _pageIndex;
            }
        }
        [JsonProperty("pindex")]
        public int PageIndex
        {
            get
            {
                return _pageIndex;
            }
            set { _pageIndex = value; }
        }
        [JsonProperty("psize")]
        public int PageSize { get { return _pageSize; } set { _pageSize = value; } }
        [JsonProperty("tpcount")]
        public int TotalPageCount
        {
            get
            {
                return _totalPageCount;
            }
        }
        [JsonProperty("list")]
        public IEnumerable<T> List { get; set; }
    }
}
