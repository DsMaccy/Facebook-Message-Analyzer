for folder in */
do
    echo $folder
    cp $folder/bin/Debug/*.dll libraries/
done
