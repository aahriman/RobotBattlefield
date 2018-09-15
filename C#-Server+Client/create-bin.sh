a=$(realpath "../../RobotBattlefield-bin/bin/")

find . -wholename "*/Release/*.dll" -exec ./create-bin-sub.sh {} $a \;
find . -wholename "*/Release/*.exe" -exec ./create-bin-sub.sh {} $a \;
