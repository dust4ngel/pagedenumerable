using System.Collections;
using System.Collections.Generic;

namespace Sandbox
{
	public interface IPagedReader<T>
	{
		List<T> Get(int skip, int take);
	}

	public class PagedEnumerable<T> : IEnumerable<T>
	{
		public class PagedEnumerator : IEnumerator<T>
		{
			private int Position;
			public int PageSize { get; set; }
			private int PageItemIndex { get { return this.Position % this.PageSize; } }

			private int LastLoadedPageIndex;
			public int PageIndex
			{
				get { return Position < 0 ? 0 : Position / PageSize; }
			}

			private IList<T> _Page;
			private IList<T> Page
			{
				get
				{
					if (this.LastLoadedPageIndex != PageIndex)
					{
						_Page = Reader.Get(this.Position, this.PageSize);
						this.LastLoadedPageIndex = this.PageIndex;
					}

					return _Page;
				}
			}

			private IPagedReader<T> Reader;

			public PagedEnumerator(IPagedReader<T> reader)
			{
				this.Reader = reader;
				this.PageSize = 10;
				this.Reset();
			}

			public T Current
			{
				get { return this.Page[this.PageItemIndex]; }
			}

			public bool MoveNext()
			{
				this.Position++;
				return this.Page.Count > this.PageItemIndex;
			}

			object System.Collections.IEnumerator.Current
			{
				get { return Current; }
			}

			public void Reset()
			{
				this.LastLoadedPageIndex = -1;
				this.Position = -1;
			}

			public void Dispose() { }
		}

		private PagedEnumerator Enumerator;

		public PagedEnumerable(IPagedReader<T> reader)
		{
			this.Enumerator = new PagedEnumerator(reader);
		}

		public IEnumerator<T> GetEnumerator()
		{
			return this.Enumerator;
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return (IEnumerator)GetEnumerator();
		}
	}
}
