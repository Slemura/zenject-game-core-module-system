namespace com.rpdev.module.common.commands {
	
	public interface ICustomCommand {
		void Execute();
	}

	public interface ICustomCommand<T> {
	    void Execute(T diskType);
	}
	
	public interface ICustomCommand<T, V> {
	    void Execute(T data, V data2);
	}
	
	public interface ICustomCommand<T, V, C> {
	    void Execute(T data, V data2, C data3);
	}
	
	
	public interface ICustomCommand<T, V, C, B> {
	    void Execute(T data, V data2, C data3, B data4);
	}
}