<?php
//Variables for the connection
	$servername = "localhost";
	$server_username =  "root";
	$server_password = "";
	$dbName = "TimeSplit";
	
//Variable from the user	
	$tmpName  = $_FILES['image']['tmp_name'];
	$fp      = fopen($tmpName, 'r');
 	$content = fread($fp, filesize($tmpName));
	$content = addslashes($content);
 	fclose($fp);
	
	//Make Connection
	$conn = new mysqli($servername, $server_username, $server_password, $dbName);
	//Check Connection
	if(!$conn){
		die("Connexion échouée. ". mysqli_connect_error());
	}
	
	$query = "INSERT INTO arbo_mj (Image) VALUES ('$content')";
 	mysqli_query($conn, $query) or die('Erreur SQL');
	$result = mysqli_query($conn ,$sql);
	
	if(!result) echo "Erreur dans l'insertion SQL";
	else echo "Insertion SQL effectuée.";

?>