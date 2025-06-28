// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


const doc = document.getElementById("ClaveRep").addEventListener("input",comprobarClave);
function comprobarClave(){
        let pass = document.getElementById("clnueva").value;
        let conf =document.getElementById("ClaveRep").value;
        let con = document.getElementById("Clave").value;
        if((pass == conf)&&con!=""){
            document.getElementById("btnClave").disabled= false;
            
        }
        else{
           document.getElementById("btnClave").disabled=true;
        }
}