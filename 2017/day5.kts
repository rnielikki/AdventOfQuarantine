import java.io.File;
/* --- part1
File("day5.txt").readLines().let{
    val arr = it.map{ it.toInt() }.toTypedArray<Int>();
    var step = 0;
    var currentIndex = 0;
    while(currentIndex>=0 && currentIndex < arr.size){
        val value = arr[currentIndex];
        arr[currentIndex]++;
        currentIndex += value;
        step++;
    }
    step
}
*/

File("day5.txt").readLines().let{
    val arr = it.map{ it.toInt() }.toTypedArray<Int>();
    var step = 0;
    var currentIndex = 0;
    while(currentIndex>=0 && currentIndex < arr.size){
        val value = arr[currentIndex];
        if(value>=3){
            arr[currentIndex]--;
        }
        else{
            arr[currentIndex]++;
        }
        currentIndex += value;
        step++;
    }
    step
}