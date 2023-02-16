import formsCss from '../forms.module.css' 
import css from './login-form.module.css'
import FormHeader from '../components/form-header'
import FormInput from '../components/form-input'
import Link from 'components/UI/Link'
import FormCheckbox from '../components/form-checkbox'
import FormSubmit from '../components/form-submit'



function LoginForm() {
    return (
        <form className={formsCss.block}>
            <FormHeader text='Вход'/>
            <div className={formsCss.inputs}>
                <FormInput title="Почта:"/>
                <FormInput title="Пароль:" type='password'/>
            </div>
            <div className={[formsCss.group, formsCss.group_between, css.group].join(' ')}>  
                <Link text="Забыли пароль?" to='/' size={1} />
                <FormCheckbox id='remember_me' text='Запомнить'/>
            </div>
            <FormSubmit text="Войти"/>
            <div className={[formsCss.group, formsCss.group_center, css.register_link].join(' ')}>
                <span className={formsCss.text}>Нет аккаунта?</span>
                <Link text="Регистрация" to='/register' size={2}/>
            </div>
        </form>
    )
}

export default LoginForm
