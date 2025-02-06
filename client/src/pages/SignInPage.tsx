
const SignInPage = () => {


    return (
        <div>
            <h1>Sign In</h1>
            <form>
                <div>
                    <label htmlFor="email">Email</label>
                    <input type="email" id="email" name="email" />
                </div>
                <div>
                    <label htmlFor="password">Password</label>
                    <input type="password" id="password" name="password" />
                </div>
                <button type="submit">Sign In</button>
            </form>
        </div>
    );
}

export default SignInPage;