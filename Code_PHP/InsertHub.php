<?php
//Variables pour la connection
	$servername = "localhost"; 
	$server_username =  "root";
	$server_password = "";
	$dbName = "TimeSplit";
	
//Variable pour l'utilisateur
	$hubID = $_POST["hubIDPost"]; 
	$hubLevel = $_POST["hubLevelPost"];
	$hubChildren = $_POST["hubChildrenPost"];
	$hubEnigme = $_POST["hubEnigmePost"];
	$hubAudio = $_POST["hubAudioPost"];
	$hubWiFiName = $_POST["hubWiFiNamePost"];
	
	//Connexion à la BDD
	$conn = new mysqli($servername, $server_username, $server_password, $dbName);
	//Verification connexion
	if(!$conn){
		die("Connection Failed. ". mysqli_connect_error());
	}
	
	$sql = "INSERT INTO arbo_mj (hubID, hubLevel, hubChildren, hubEnigme, hubAudio, hubImage, hubWifiName)
			VALUES ('".$hubID."','".$hubLevel."','".$hubChildren."','".$hubEnigme."','".$hubAudio."','".$hubWiFiName."')";
	$result = mysqli_query($conn ,$sql);
	
	if(!result) echo "Erreur dans l'insertion SQL";
	else echo "Insertion SQL effectuée.";

?>