function part1(raw) {
   let register = new Map();
   let opList = [];

   raw.split("\n").forEach(str => {
      str=str.trim(); //fuck the \r
      if (str === "") return;
      if (str.indexOf(':') < 0) {
         let v = str.split(" ");
         opList.push({ val1: v[0], val2: v[2], op: v[1], result: v[4] });
      }
      else {
         let spl = str.split(':');
         register.set(spl[0], Number(spl[1].trim()))
      }
   })

   let unused = new Set();

   do {
      for (let i = 0; i < opList.length; i++) {
         let current = opList[i];
         calc(current.val1, current.val2, current.result, current.op, i);
      }
   } while (unused.size > 0);


   console.log(getNum("z") - (getNum("x")+getNum("y")));
   console.log(Math.log2(Math.abs(getNum("z") - (getNum("x")+getNum("y")))));
   return {
      instructions: opList,
      register: register
   };
   function getNum(start) {
      var zArr = [...register.keys()].filter(a => a.startsWith(start)).map(a => Number(a.substring(1))).filter(v => !isNaN(v)).sort((a, b) => b - a)
      let sumStr = "";
      for (let z of zArr) {
         let zVar = start + z.toString().padStart(2, "0");
         if (!register.has(zVar)) throw zVar;
         sumStr += register.get(zVar).toString(); //full of regret for using js
      }
      return parseInt(sumStr, 2);
   }

   function calc(val1, val2, result, op, lineNumber) {
      let v1 = findVariable(val1);
      let v2 = findVariable(val2);
      if (v1 === null || v2 === null) {
         unused.add(lineNumber);
         return;
      }
      else if (unused.has(lineNumber)) unused.delete(lineNumber);
      switch (op) {
         case "AND":
            saveVariable(result, v1 & v2);
            break;
         case "XOR":
            saveVariable(result, v1 ^ v2);
            break;
         case "OR":
            saveVariable(result, v1 | v2);
            break;
         default:
            throw "invalid operation from " + op;
      }
   }

   function findVariable(name) {
      if (register.has(name)) return register.get(name);
      return null;
   }

   function saveVariable(name, value) {
      register.set(name, value);
   }
}
function splitByCondition(input, condition) {
   return input.reduce((r, s) => {
      condition(s) ? r[0].push(s) : r[1].push(s);
      return r;
   }, [[], []]);
}
