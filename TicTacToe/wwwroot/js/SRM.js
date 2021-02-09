"use strict";

var mconnection = new signalR.HubConnectionBuilder().withUrl("/MESHUB").build();

var mReconnectTimeout = 1;
var mReconnectTimeoutIncrement = 1;
var mMaxReconnectInterval = 600;

mconnection.on("ChatUpdate", function (msg)
{
	ChatApp.getdata();
});

mconnection.onclose(function ()
{
	mstart();
});

function mstart()
{
	mconnection.start().then(function ()
	{
		mReconnectTimeout = 1;
		setcounter();
	}).catch(function (err)
	{
		var mtimeout = mReconnectTimeout * 1000;
		setTimeout(mstart, mtimeout);
		if (mReconnectTimeout < mMaxReconnectInterval)
		{
			mReconnectTimeout += mReconnectTimeoutIncrement;
		}
		msetcounter();

		//return console.error(err.toString());
	});
}

mstart();

function msetcounter()
{
	//document.getElementById("cntr").innerHTML = ReconnectTimeout;
}