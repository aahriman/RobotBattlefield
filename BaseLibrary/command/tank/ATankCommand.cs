using System;
using BaseLibrary.visitors;

namespace BaseLibrary.command.tank {
    public abstract class ATankCommand : ACommand{
        /// <summary>
        /// This is not supported. But if accepter is <code>ITankCommandVisitor</code> then it call <code>accept(ITankCommandVisitor accepter)</code> otherwise throw exception <code>NotImplementedException</code>
        /// </summary>
        /// <exception cref="NotImplementedException">if accepter is not ITankCommandVisitor</exception>
        /// <param name="accepter"></param>
        public sealed override void accept(ICommandVisitor accepter) {
            if (accepter is ITankCommandVisitor) {
                this.accept((ITankCommandVisitor)accepter);
            } else {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// This is not supported. But if accepter is <code>ITankCommandVisitor&lt;Output&gt</code> then it call <code>&lt;Output&gt; accept&lt;Output&gt;(ITankCommandVisitor&lt;Output&gt; accepter)</code> otherwise throw exception <code>NotImplementedException</code>
        /// </summary>
        // <exception cref="NotImplementedException">if accepter is not ITankCommandVisitor&lt;Output&gt;</exception>
        /// <param name="accepter"></param>
        public sealed override Output accept<Output>(ICommandVisitor<Output> accepter) {
            if (accepter is ITankCommandVisitor<Output>) {
                return this.accept((ITankCommandVisitor<Output>)accepter);
            } else {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// This is not supported. But if accepter is <code>ITankCommandVisitor&lt;Output Intput&gt;</code> then it call <code>&lt;Output&gt; accept&lt;Output&gt;(ITankCommandVisitor&lt;Output, Input&gt; accepter, Input input)</code> otherwise throw exception <code>NotImplementedException</code>
        /// </summary>
        /// <exception cref="NotImplementedException">if accepter is not ITankCommandVisitor&lt;Output Intput&gt;</exception>
        /// <param name="accepter"></param>
        public sealed override Output accept<Output, Input>(ICommandVisitor<Output, Input> accepter, Input input) {
            if (accepter is ITankCommandVisitor<Output, Input>) {
                return this.accept((ITankCommandVisitor<Output, Input>) accepter, input);
            } else {
                throw new NotImplementedException();
            }
        }

        public abstract void accept(ITankCommandVisitor accepter);

        public abstract Output accept<Output>(ITankCommandVisitor<Output> accepter);


        public abstract Output accept<Output, Input>(ITankCommandVisitor<Output, Input> accepter, Input input);
    }
}
