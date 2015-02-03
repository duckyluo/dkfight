using System;

public interface IProcess
{
	void Start();
	void Stop();
	void Update();
	void End();
	void Destroy();
	eProcessStatus GetStatus();
	bool IsRunning();
}