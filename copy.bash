#! /bin/bash

XAM_SRC="/Users/andy/school/game-design/xamarin/Escape/Escape"
GIT_LOC="/Users/andy/school/game-design/Escape/Escape"

for f in $@; do
  if [[ -f $XAM_SRC/$f ]]; then
    cp $XAM_SRC/$f $GIT_LOC/$f
  else
    echo "$XAM_SRC/$f not found; skipping"
  fi
done