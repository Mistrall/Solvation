using System;
using System.Collections.Generic;

namespace Solvation.Domain.SpecialCollections
{
	public class Heap<T>
	{
		protected readonly IList<T> list;
		private readonly IComparer<T> comparer;
		public int Count { get; private set; }

		public Heap(IList<T> list, int count, IComparer<T> comparer)
		{
			this.list = list;
			this.comparer = comparer;
			Count = count;
			Heapify();
		}

		public void Insert(T e)
		{
			list.Add(e);
			Count++;
			HeapUp(Count - 1);
		}

		public T PopRoot()
		{
			if (Count == 0) throw new InvalidOperationException("Empty heap.");
			var root = list[0];
			SwapHeapElements(0, Count - 1);
			list.RemoveAt(Count - 1);
			Count--;
			HeapDown(0);
			return root;
		}

		public T PeekRoot()
		{
			if (Count == 0) throw new InvalidOperationException("Empty heap.");
			return list[0];
		}

		public void DeleteAt(int i)
		{
			if (Count == 0) throw new InvalidOperationException("Empty heap.");
			if (i < 0 || i > Count) throw new InvalidOperationException("No element with such index.");
			SwapHeapElements(i, Count - 1);
			list.RemoveAt(Count - 1);
			Count--;
			HeapDown(i);
		}

		private void Heapify()
		{
			for (int i = ParentIndex(Count - 1); i >= 0; i--)
			{
				HeapDown(i);
			}
		}

		private void HeapUp(int i)
		{
			T el = list[i];
			while (true)
			{
				int parent = ParentIndex(i);
				if (parent < 0 || comparer.Compare(list[parent], el) > 0) break;
				SwapHeapElements(i, parent);
				i = parent;
			}
		}

		private void HeapDown(int i)
		{
			while (true)
			{
				int lchild = LeftChildIndex(i);
				if (lchild < 0) break;
				int rchild = RightChildIndex(i);

				int child = rchild < 0
								? lchild
								: comparer.Compare(list[lchild], list[rchild]) > 0 ? lchild : rchild;

				if (comparer.Compare(list[child], list[i]) < 0) break;
				SwapHeapElements(i, child);
				i = child;
			}
		}

		private int ParentIndex(int i)
		{
			return i <= 0 ? -1 : SafeIndex((i - 1) / 2);
		}

		private int RightChildIndex(int i)
		{
			return SafeIndex(2 * i + 2);
		}

		private int LeftChildIndex(int i)
		{
			return SafeIndex(2 * i + 1);
		}

		private int SafeIndex(int i)
		{
			return i < Count ? i : -1;
		}

		protected virtual void SwapHeapElements(int i, int j)
		{
			T temp = list[i];
			list[i] = list[j];
			list[j] = temp;
		}
	}
}
