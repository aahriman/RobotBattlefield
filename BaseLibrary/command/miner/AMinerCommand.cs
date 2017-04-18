using System;
using BaseLibrary.visitors;

namespace BaseLibrary.command.miner {
    public abstract class AMinerCommand : ACommand {
        /// <summary>
        /// This is not supported. But if accepter is <code>IMinerCommandVisitor</code> then it call <code>accept(ITankCommandVisitor accepter)</code> otherwise throw exception <code>NotImplementedException</code>
        /// </summary>
        /// <exception cref="NotImplementedException">if accepter is not IMinerCommandVisitor</exception>
        /// <param name="accepter"></param>
        public sealed override void accept(ICommandVisitor accepter) {
            if (accepter is IMinerCommandVisitor) {
                this.accept((IMinerCommandVisitor)accepter);
            } else {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// This is not supported. But if accepter is <code>IMinerCommandVisitor&lt;Output&gt</code> then it call <code>&lt;Output&gt; accept&lt;Output&gt;(IMinerCommandVisitor&lt;Output&gt; accepter)</code> otherwise throw exception <code>NotImplementedException</code>
        /// </summary>
        // <exception cref="NotImplementedException">if accepter is not IMinerCommandVisitor&lt;Output&gt;</exception>
        /// <param name="accepter"></param>
        public sealed override Output accept<Output>(ICommandVisitor<Output> accepter) {
            if (accepter is IMinerCommandVisitor<Output>) {
                return this.accept((IMinerCommandVisitor<Output>)accepter);
            } else {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// This is not supported. But if accepter is <code>IMinerCommandVisitor&lt;Output Intput&gt;</code> then it call <code>&lt;Output&gt; accept&lt;Output&gt;(IMinerCommandVisitor&lt;Output, Input&gt; accepter, Input input)</code> otherwise throw exception <code>NotImplementedException</code>
        /// </summary>
        /// <exception cref="NotImplementedException">if accepter is not IMinerCommandVisitor&lt;Output Intput&gt;</exception>
        /// <param name="accepter"></param>
        public sealed override Output accept<Output, Input>(ICommandVisitor<Output, Input> accepter, Input input) {
            if (accepter is IMinerCommandVisitor<Output, Input>) {
                return this.accept((IMinerCommandVisitor<Output, Input>)accepter, input);
            } else {
                throw new NotImplementedException();
            }
        }

        public abstract void accept(IMinerCommandVisitor accepter);

        public abstract Output accept<Output>(IMinerCommandVisitor<Output> accepter);


        public abstract Output accept<Output, Input>(IMinerCommandVisitor<Output, Input> accepter, Input input);
    }
}
