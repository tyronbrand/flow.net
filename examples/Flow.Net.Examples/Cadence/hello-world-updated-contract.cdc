access(all) contract HelloWorld {
    access(all) let greeting: String
    init()
    {
        self.greeting = "Hello world! .Net is the best!"
    }
    access(all) fun hello(): String {
        return self.greeting
    }
}