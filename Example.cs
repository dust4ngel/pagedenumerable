using System.Collections;
using System.Collections.Generic;

namespace Sandbox
{
	/// <summary>
	/// Example implementation of IPagedReader
	/// </summary>
	public class PagedNumberReader : IPagedReader<int>
	{
		public List<int> Get(int skip, int take)
		{
			Console.WriteLine("Loading {0} .. {1}", skip, skip + take);
			if (skip >= 100)
				return new List<int>();
			else
				return Enumerable.Range(skip, take).ToList();
		}
	}
	
	class Program
	{
		static void Main(string[] args)
		{
			var nums = new PagedEnumerable<int>(new PagedNumberReader());
			foreach (var n in nums.Select(i => 2*i))
			{
				Console.WriteLine(n);
			}

			Console.WriteLine("Complete.");
			Console.ReadKey();
			return;
		}
}
