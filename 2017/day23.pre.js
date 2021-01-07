const raw = document.querySelector("pre").textContent.split("\n").filter(x=>x);
var codes = raw.map(l=>
l
.replace(/set ([a-z]) (-?\d+|[a-z])/g,"$1 = $2;")
.replace(/sub ([a-z]) (-?\d+|[a-z])/g,"$1 -= $2;")
.replace(/mul ([a-z]) (-?\d+|[a-z])/g,"$1 *= $2;")
.replace(/jnz (-?\d+|[a-z]) (-\d+)/g, "}while($1!=0);_$2")
.replace(/jnz (-?\d+|[a-z]) (\d+)/g, "if($1==0){_$2")
//.replace(/jnz (-?\d+) (\d+)/g, "_$1_$2")
);
let len = codes.length;
for(let i=0;i<len;i++){
   let code = codes[i];
   if(code[0]=='}'){
      let split = code.split('_');
      let lineNumber = i+Number(split[1]);
      codes[lineNumber] = "do{" + codes[lineNumber];
      codes[i] = split[0];
   }
   else if(code.substring(0,2)=='if'){
      let split = code.split('_');
      let offset = Number(split[1]);
      if(offset == 2 && codes[i+1].substring(0,4) == "if(1"){
          codes[i]=split[0].replace("==","!=");
          let endIndex = Number(codes[i+1].split('_')[1])+i;
          codes[endIndex] = codes[endIndex]+"}";
          codes[i+1] = "";
          i++;
      }
     else{
      let lineNumber = i+offset;
      codes[lineNumber] = "}" + (codes[lineNumber] ?? "");
      codes[i] = split[0];
     }
   }
/*
   else if(code[0]=='_'){
      let split = code.split('_');
      split.shift();
      if(Number(split[0])==0) continue;
      let count = Number(split[1]);
      let lineNumber = i+Number(split[1]);
      while(count-- >0){
         codes[i+count] = "";
      }
   }*/
}

console.log(codes.join("\n"));

eval("var a=0;var b=0;var c=0;var d=0;var e=0;var f=0; var g=0; var h=0;"+codes.join("\n"));