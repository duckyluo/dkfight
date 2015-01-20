using System;
using System.Diagnostics;
using UnityEngine;

public class DkGameTimer
{
	protected static bool m_runEnable = false;

	protected static Stopwatch stopwatch = new Stopwatch();

	protected static float m_lastTime;
	public static float LastTime
	{
		get{return m_lastTime;}
	}

	protected static float m_elapsedTime = 0;
	public static float ElapsedTime
	{
		get{return m_elapsedTime;}
	}

	private static float m_deltaTime;
	public static float DeltaTime
	{
		get{return m_deltaTime;}
	}

	public static float DeltaSeconds
	{
		get{return m_deltaTime/1000f;}
	}

	public static void Start()
	{
		m_runEnable = true;
		m_elapsedTime = 0;
		stopwatch.Start();
	}

	public static void Pause()
	{
		m_runEnable = false;
		stopwatch.Stop();
	}

	public static void Restart()
	{
		m_runEnable = true;
		stopwatch.Restart();
	}

	public static void Update()
	{
		if(m_runEnable)
		{
			m_lastTime = m_elapsedTime;
			m_elapsedTime = stopwatch.ElapsedMilliseconds;
			m_deltaTime = m_elapsedTime - m_lastTime;
		}
	}
}


//	private static Stopwatch stopwatch = new Stopwatch();
//
//	private static float lastTime;
//	public static float LastTime
//	{
//		get{return lastTime;}
//	}
//
//	private static float deltaTime;
//	public static float DeltaTime
//	{
//		get{return deltaTime;}
//	}
//
//	public static float DeltaSeconds
//	{
//		get{return deltaTime/1000f;}
//	}
//
//	private static float gameElapsedTime;
//	public static float GameElapsedTime
//	{
//		get{return gameElapsedTime;}
//	}
//
//	public static bool IsRunning()
//	{
//		return stopwatch.IsRunning;
//	}
//
//	public static void Reset()
//	{
//		stopwatch.Reset();
//	}
//
//	public static void Start()
//	{
//		stopwatch.Start();
//	}
//
//	public static void Stop()
//	{
//		stopwatch.Stop();
//	}
//
//	public static float Update()
//	{
//		lastTime = gameElapsedTime;
//		gameElapsedTime = stopwatch.ElapsedMilliseconds;
//		deltaTime = gameElapsedTime - lastTime;
//		return deltaTime;
//	}

