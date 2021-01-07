import java.io.File;
/*

File("day23.txt").readLines().let{
    val variables = HashMap<Char, Int>();
    var multiple = 0;
    for(i in 0..7){
        variables.put((97+i).toChar(),0);
    }
    fun getVar(value:String):Int{
        return (value.toIntOrNull() ?: variables[value[0]])!!;
    }
    var currentLine = 0;
    while(currentLine < it.size){
        val cut = it[currentLine].split(' ');
        val target = cut[1];
        var value = getVar(cut[2]);
        when(cut[0]){
            "set"->
                variables.set(target[0], value);
            "sub"->
                variables.set(target[0], variables.get(target[0])!!-value);
            "mul"->
                { multiple++; variables.set(target[0], variables.get(target[0])!!*value); }
            "jnz"->{
                    if(getVar(cut[1])!=0)
                        currentLine += value-1;
                }
            else-> throw Exception("file format wrong");
        } 
        currentLine++;
    }
    multiple;
} */
File("day23.txt").readLines().let{
    //found that b-c should be zero.
    //can be vary depends on input!!
    val lastLine = it.last();
    val lineOffset = lastLine.substring(lastLine.lastIndexOf(' ')+1).toIntOrNull()!!;
    val loopPart = it.size - 1 + lineOffset;

    val (before, after) = it.withIndex()
    .partition{
        val (index, _) = it;
        index < loopPart
    }
    val beforeLoop=before
    .filter{
        val (_, value) = it;
        (value.indexOf(" b ")!=-1 || (value.indexOf(" c ")!=-1 || (value.indexOf(" h ")!=-1)))
    }.map{
        val (_, value) = it;
        value.split(' ')
    };
    val afterLoop=after
    .map{
        val (_, value) = it;
        value.split(' ')
    }
    .dropLast(1).takeLastWhile{ it[1]!="h" };

    val variables = HashMap<Char, Int>();
    variables.put('b',0);
    variables.put('c',0);
    variables.put('h',0);
    variables.put('g',0);

    fun getVar(value:String):Int{
        return (value.toIntOrNull() ?: variables[value[0]])!!;
    }
    fun calc(instruct:String, target:Char, value:Int){
        when(instruct){
            "set"->
                variables.set(target, value);
            "sub"->
                variables.set(target, variables.get(target)!!-value);
            "mul"->
                variables.set(target, variables.get(target)!!*value);
            "jnz"->{}
            else-> throw Exception("file format wrong");
        }
    }
    beforeLoop.forEach{
        val target = it[1][0];
        var value = getVar(it[2]);
        calc(it[0], target, value);
    }
    println(variables['b']);
    println(variables['c']);
    //while(variables['b']!!-variables['c']!! !=0){
    do{
        variables['h'] = variables['h']!!+1;
        afterLoop.forEach{
            //println(it);
            val target = it[1][0];
            var value = getVar(it[2]);
            if(it[0]=="jnz" && it[1]=="g"){
                if(variables['g']==0){
                    println(variables['h']);
                }
            }
            calc(it[0], target, value);
        }
    }while(variables['g']!! !=0);
    variables['h'];
}