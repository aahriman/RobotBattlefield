using System.Collections.Generic;
using CommunicationLibrary.command;

namespace CommunicationLibrary.visitors {
	public abstract class AVisitorGUI : AVisitorCommand {
		/// <summary>
		/// Remove* methods use only if you are 100% sure
		/// </summary>
		protected List<Bitmap> bitmaps;
		protected int bitmapDrawingIndex = 0;
		protected int bitmapIndex = 0;

		protected AVisitorGUI() {
			bitmaps = new List<Bitmap>();
		}

		protected AVisitorGUI(Bitmap[] bitmaps) {
			this.bitmaps = new List<Bitmap>(bitmaps);
		}

		public Bitmap getActualDrawingBitmap() {
			return bitmaps[bitmapDrawingIndex];
		}

		public Bitmap getNextDrawingBitmap() {
			bitmapDrawingIndex = getNextIndex(bitmapDrawingIndex);
			lock (bitmaps) {
				return bitmaps[bitmapDrawingIndex];
			}
		}

		protected void nextBitmap() {
			bitmapIndex = getNextIndex(bitmapIndex);
		}


		public override void visit(RobotStateCommand visitor) {
			nextBitmap();
		}

		private int getNextIndex(int index) {
			lock (bitmaps) { 
				return (index + 1) % bitmaps.Count;
			}
		}
	}
}
