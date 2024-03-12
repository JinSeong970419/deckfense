echo $CI_COMMIT_SHORT_SHA
echo "current branch is $CI_COMMIT_BRANCH"
set username
echo $Env:USERNAME 
git config --global --add safe.directory "$UNITY_DIR_WIN"
git -C "$UNITY_DIR_WIN" status
git -C "$UNITY_DIR_WIN" checkout .
git -C "$UNITY_DIR_WIN" remote update
git -C "$UNITY_DIR_WIN" checkout $CI_COMMIT_BRANCH
git -C "$UNITY_DIR_WIN" pull

pause