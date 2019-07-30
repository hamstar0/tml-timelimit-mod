namespace TimeLimit.NetProtocol {
	public enum TimeLimitProtocolTypes : byte {
		RequestTimers,
		TimerStart,
		TimersStop,
		TimersPause,
		TimersResume,
		TimersAllStop,
		TimersAllPause,
		TimersAllResume
	}
}
