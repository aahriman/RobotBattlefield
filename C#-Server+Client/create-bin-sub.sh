echo "$1 -> $2";
a=$1;
a=${a/*\///};
cp -f $1 "$2/$a";
