import css from './formFileInfo.module.css'
import Types from './components/formFileTypes';
import { parse_time } from './components/utils';



const FormFileInfo = ({ data, name, type, guid, update, setFilesInfo }) => {
    const Elem = Types[type?.name] || Types.Folder
    return (
        <div className={css.block} onClick={evt => evt.stopPropagation()} onContextMenu={e => e.stopPropagation()}>
            {data &&
                <>
                    <div className={css.header}>
                        <h3 className={css.title}>{name}</h3>
                    </div>

                    <div className={css.body}>
                        <Elem {...{ ...data, creationTime: parse_time(data.creationTime) }} id={guid} setFilesInfo={setFilesInfo} update={update} />
                    </div>
                </>

            }
        </div>
    )

}

export default FormFileInfo;
