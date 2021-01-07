const content = document.querySelector("pre").textContent;
const calc = content.replace("\n\n","\nlet tape=new Set();\nvar cursor=0;\n").
replace(/Begin in state ([A-Z])\./, "let start = '$1';").
replace(/Perform a diagnostic checksum after (\d+) steps./, "let MAX_STEPS = $1;\nvar currentStep = 0;").
replace(/In state ([A-Z]):/g, "let $1=function(){").
replaceAll("\n\n","\n}\n").
replace(/If the current value is (\d):/g,"if(!(tape.has(cursor)^$1)){")
.replace(/- Write the value (\d)\./g,"$1?tape.add(cursor):tape.delete(cursor);")
.replace(/- Move one slot to the (\w+)\./g,"('$1'=='left')?cursor--:cursor++;")
.replace(/- Continue with state ([A-Z])\./g, "currentStep++; if(currentStep>MAX_STEPS) return; else $1();}")
+"}\neval(start+'()');\nconsole.log(tape.size);"
eval(calc);