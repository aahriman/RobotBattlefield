
package BaseLibrary.communication.command.common;

import BaseLibrary.communication.protocol.AProtocol;

public class ScanAnswerCommand extends ACommonCommand{

    	private static final String NAME = "SCAN_ANSWER";

    	static {
    		AProtocol.registerForDeserialize(NAME, ScanAnswerCommand.class);
    	}

    	@Override
    	public String getCommandName() {
    		return NAME;
    	}

        private double range;
        /**
        * Distance to robot which was scanned.
        */
        public double getRange() {
        	if (pending)
    			throw new UnsupportedOperationException("Cannot access to property of pending request.");
        	return range;
        }
            

        private int enemyID;
        /**
        * Robot's which was scanned id.
        */
        public int getEnemyID() {
        	if (pending)
    			throw new UnsupportedOperationException("Cannot access to property of pending request.");
        	return enemyID;
        }
            

        public ScanAnswerCommand() { }

        public ScanAnswerCommand(double range, int enemyID) {
                this.enemyID = enemyID;
                	this.range = range;
            pending = false;
        }

        public void FillData(ScanAnswerCommand source) {
            this.enemyID = source.enemyID;
            this.range = source.range;
            pending = false;
        }
   }
