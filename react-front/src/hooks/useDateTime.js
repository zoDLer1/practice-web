
import useValidateInput from './useValidateInput'
import dateFormat from 'dateformat'

function useDateTime({ value, validation_methods }) {



    const { onChanged } = useValidateInput({ value, ...validation_methods })
    

    const getTime = () => ({
        value: value ? dateFormat(value, 'hh:MM') : '',
        onChange: (evt) => {
            const [hours, minutes] = evt.target.value.split(':')
            const updatedTime = value ? new Date(value) : new Date()
            updatedTime.setHours(hours)
            updatedTime.setMinutes(minutes)
            onChanged(updatedTime)
        }
    })

    const getDate = () => ({
        value: value ? dateFormat(value, 'yyyy-mm-dd') : '',
        onChange: (evt) => 
        {
            const [year, month, day] = evt.target.value.split("-")
            let updatedTime = new Date(value)
            if (!value){
                updatedTime = new Date(year, month, day, 1, 0)
            }
            else{
                updatedTime.setFullYear(year)
                updatedTime.setMonth(month)
                updatedTime.getDate(day)
            }

            return onChanged(updatedTime)
        }
        
    })


    return { getTime, getDate }
}

export default useDateTime
