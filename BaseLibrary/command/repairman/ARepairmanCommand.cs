using System;
using BaseLibrary.visitors;

namespace BaseLibrary.command.repairman {
    public abstract class ARepairmanCommand : ACommand{
        /// <summary>
        /// This is not supported. But if accepter is <code>IRepairmanCommandVisitor</code> then it call <code>accept(IRepairmanCommandVisitor accepter)</code> otherwise throw exception <code>NotImplementedException</code>
        /// </summary>
        /// <exception cref="NotImplementedException">if accepter is not IRepairmanCommandVisitor</exception>
        /// <param name="accepter"></param>
        public sealed override void accept(ICommandVisitor accepter) {
            if (accepter is IRepairmanVisitor) {
                this.accept((IRepairmanVisitor)accepter);
            } else {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// This is not supported. But if accepter is <code>IRepairmanCommandVisitor&lt;Output&gt</code> then it call <code>&lt;Output&gt; accept&lt;Output&gt;(IRepairmanCommandVisitor&lt;Output&gt; accepter)</code> otherwise throw exception <code>NotImplementedException</code>
        /// </summary>
        // <exception cref="NotImplementedException">if accepter is not IRepairmanCommandVisitor&lt;Output&gt;</exception>
        /// <param name="accepter"></param>
        public sealed override Output accept<Output>(ICommandVisitor<Output> accepter) {
            if (accepter is IRepairmanVisitor<Output>) {
                return this.accept((IRepairmanVisitor<Output>)accepter);
            } else {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// This is not supported. But if accepter is <code>IRepairmanCommandVisitor&lt;Output Intput&gt;</code> then it call <code>&lt;Output&gt; accept&lt;Output&gt;(IRepairmanCommandVisitor&lt;Output, Input&gt; accepter, Input input)</code> otherwise throw exception <code>NotImplementedException</code>
        /// </summary>
        /// <exception cref="NotImplementedException">if accepter is not IRepairmanCommandVisitor&lt;Output Intput&gt;</exception>
        /// <param name="accepter"></param>
        public sealed override Output accept<Output, Input>(ICommandVisitor<Output, Input> accepter, Input input) {
            if (accepter is IRepairmanVisitor<Output, Input>) {
                return this.accept((IRepairmanVisitor<Output, Input>)accepter, input);
            } else {
                throw new NotImplementedException();
            }
        }

        public abstract void accept(IRepairmanVisitor accepter);

        public abstract Output accept<Output>(IRepairmanVisitor<Output> accepter);


        public abstract Output accept<Output, Input>(IRepairmanVisitor<Output, Input> accepter, Input input);
    }
}
