#!/bin/bash
PORT=/dev/cu.usbmodem1421
while true 
do 
	echo -n "~U" > $PORT 
	sleep 1  
done
