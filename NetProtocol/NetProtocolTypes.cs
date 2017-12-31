namespace TimeLimit.NetProtocol {
	public enum TimeLimitProtocolTypes : byte {
		RequestModSettings,
		ModSettings,
		RequestTimers,
		TimerSend,
		EndTimers
	}
}
