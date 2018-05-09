using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using RobotArena;
using RobotArena.arena;

namespace RobotViewer.gui {
	public partial class ArenaGUI : Form {
		/*public static readonly double FPS = 24.5;

		private readonly Queue<ArenaInfoStore> arenaInfoStores;
		private readonly IArena arena;
		private bool end = false;

		private readonly Bitmap[] bitmaps;
		private int index = 0;

		protected readonly List<BulletInfo>[] expodedBullets = { new List<BulletInfo>(), new List<BulletInfo>() };

		private Drawer drawer;

		SemaphoreSlim semaphore = new SemaphoreSlim(0);

		public ArenaGUI(IArena arena) : base() {
			InitializeComponent();
			this.arena = arena;
			arena.AddListener(this);
			arenaInfoStores = new Queue<ArenaInfoStore>();
			bitmaps = new Bitmap[2];
			drawer = new DefaultDrawer();
		}

		public async void Listen(IArenaInfo info) {
			await Task.Yield();
			
			if (ArenaInfoAction.INIT.Equals(info.GetAction())) {
				DateTime date = DateTime.Now;
				ArenaInfoStore item = info as ArenaInfoStore;
				if (item != null) {
					oneFrame(ref date, item);
				} else {
					oneFrame(ref date, new ArenaInfoStore(info));	
				}
			}else if (ArenaInfoAction.NEW_LAP.Equals(info.GetAction())) {
				ArenaInfoStore item = info as ArenaInfoStore;
				if (item != null) {
					lock (arenaInfoStores) {
						arenaInfoStores.Enqueue(item);
						semaphore.Release();
					}
				} else {
					lock (arenaInfoStores) {
						arenaInfoStores.Enqueue(new ArenaInfoStore(info));
						semaphore.Release();
					}
				}
			} else if (ArenaInfoAction.END_MATCH.Equals(info.GetAction())) {
				end = true;
				arena.RemoveListener(this);
			}
		}

		public void setDrawFactory(Drawer drawer) {
			if (drawer == null) {
				throw new ArgumentNullException("drawer");
			}
			this.drawer = drawer;
		}


		private void oneFrame(ref DateTime start, ArenaInfoStore ai) {
			pictureArena.BeginInvoke(new InvokeDelegate(() => {
				draw(ai);
				index = getNewIndex();
				pictureArena.Image = bitmaps[index];
				pictureArena.Invalidate();
			}));
			
			RobotInfo[] scoreboard = ai.GetRobotsInfo();
			Array.Sort(scoreboard, (r1, r2) => r2.SCORE - r1.SCORE);

			this.listBox1.BeginInvoke(new InvokeDelegate(() => {
				listBox1.Items.Clear();
				foreach (var scoreboardItem in scoreboard) {
					this.listBox1.Items.Add(String.Format("{0}:{1}", scoreboardItem.NAME, scoreboardItem.SCORE));
				}
				this.listBox1.Invalidate();
			}));


			int sleep = (int)((1000.0 / FPS) - (start - DateTime.Now).TotalMilliseconds);
			Thread.Sleep(sleep);
			start = DateTime.Now;
		}

		public delegate void InvokeDelegate();
		public void draw() {
			Task t = new Task(() => {
				while (!end) {
					if (arenaInfoStores.Count >= FPS && this.Visible) {
						DateTime start = DateTime.Now;
						for (int i = 0; i < arenaInfoStores.Count && this.Visible; i++) {
							ArenaInfoStore ai;
							lock (arenaInfoStores) {
								ai = arenaInfoStores.Dequeue();
							}
							oneFrame(ref start, ai);
						}
					} else {
						semaphore.Wait();
					}
				}
				{
					DateTime start = DateTime.Now;
					while (arenaInfoStores.Count > 0) {
						lock (arenaInfoStores) {
							ArenaInfoStore ai;
							lock (arenaInfoStores) {
								ai = arenaInfoStores.Dequeue();
							}
							oneFrame(ref start, ai);
						}
					}
				}
			});
			t.Start();
		}

		protected virtual void draw(IArenaInfo info, Graphics graphics) {
			int newIndex = getNewIndex();
			expodedBullets[newIndex].Clear();
			foreach (var explodedBullet in expodedBullets[index]) {
				if (drawer.drawExplodedBullet(explodedBullet, graphics, info.GetLap())) {
					expodedBullets[newIndex].Add(explodedBullet);
				}
			}
			foreach (RobotInfo r in info.GetRobotsInfo()) {
				drawer.drawRobot(r, graphics);
			}
			
			foreach (BulletInfo bullet in info.GetBulletsInfo()) {
				if (bullet.EXPLODED) {
					if (drawer.drawExplodedBullet(bullet, graphics, info.GetLap())) {
						expodedBullets[newIndex].Add(bullet);	
					}
				} else {
					drawer.drawBullet(bullet, graphics);	
				}
			}
		}

		private void draw(IArenaInfo ai) {
			Bitmap bitmap = nextBitmap();
			using (var graphics = Graphics.FromImage(bitmap)) {
				graphics.Clear(Color.White);
				graphics.ScaleTransform(bitmap.Width/1000F, bitmap.Height/1000F);
				draw(ai, graphics);
			}
		}

		protected Bitmap nextBitmap() {
			Bitmap ret = bitmaps[getNewIndex()];
			if (ret == null || ret.Height != this.pictureArena.Height || ret.Width != this.pictureArena.Width) {
				bitmaps[getNewIndex()] = new Bitmap(this.pictureArena.Width, this.pictureArena.Height);
			}
			return bitmaps[getNewIndex()];
		}

		private int getNewIndex() {
			return (index + 1)%bitmaps.Length;
		} */
	}
}