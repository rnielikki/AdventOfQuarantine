import java.io.File;

File("day18.txt").readLines().let{
    val variables = HashMap<Char, Long>();
    /*
    for(i in 0..7){
        variables.put((97+i).toChar(),0);
    } */
    fun getVar(value:String):Long{
        var tryGet = value.toLongOrNull();
        if(tryGet==null){
            if(!variables.contains(value[0])){
                variables.put(value[0],0);
                return 0;
            }
            else{
                return variables[value[0]]!!;
            }
        }
        else{
            return tryGet;
        }
    }
    var currentLine:Long = 0;
    var sound:Long = 0;
    while(currentLine < it.size){
        val cut = it[currentLine.toInt()].split(' ');
        val target = cut[1];
        var value = 0L;
        if(cut.size>2){
            value = getVar(cut[2]);
        }
        when(cut[0]){
            "set"->
                variables.set(target[0], value);
            "add"->
                variables.set(target[0], getVar(target)+value);
            "mul"->
                variables.set(target[0], getVar(target)*value);
            "mod"->
                variables.set(target[0], getVar(target)%value);
            "snd"->{
                sound = getVar(target)
            }
            "rcv"->{
                    if(getVar(target)!=0L){
                        println(sound);
                        return@let;
                    }
                }
            "jgz"->{
                    if(getVar(cut[1])>0L)
                        currentLine += value-1;
                }
            else-> throw Exception("file format wrong");
        } 
        currentLine++;
    }
}