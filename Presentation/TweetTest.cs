using System.Collections;
using System.Collections.Generic;
using naichilab;
using UnityEngine;

public class TweetTest : MonoBehaviour {

	public void Test() {
		UnityRoomTweet.Tweet("eelighttest", "tweetSample");
	}
	
	public void Tweet()
	{
		UnityRoomTweet.Tweet("eelighttest", "ツイートサンプルです。");
	}

	public void TweetWithHashtag()
	{
		UnityRoomTweet.Tweet("eelighttest", "ツイートサンプルです。", "unityroom");
	}

	public void TweetWithHashtags()
	{
		UnityRoomTweet.Tweet("eelighttest", "ツイートサンプルです。", "unityroom", "unity1week");
	}

	public void TweetWithImage()
	{
		UnityRoomTweet.TweetWithImage("eelighttest", "ツイートサンプルです。");
	}

	public void TweetWithHashtagAndImage()
	{
		UnityRoomTweet.TweetWithImage("eelighttest", "ツイートサンプルです。", "unityroom");
	}

	public void TweetWithHashtagsAndImage()
	{
		UnityRoomTweet.TweetWithImage("eelighttest", "ツイートサンプルです。", "unityroom", "unity1week");
	}

}