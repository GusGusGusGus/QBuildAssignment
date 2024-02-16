import { useEffect, useState } from 'react';
import './App.css';
import TreeView from './components/TreeView';

interface DetailedPart {
    id: number;
    guid: string;
    parentName: string;
    componentName: string;
    partNumber: string;
    title: string;
    quantity: number;
    type: string;
    item: string;
    material: string;
}


export interface Node {
    id: number,
    guid: string,
    children: Node[],
    name: string,
    parentName: string;
    partNumber: string;
    title: string;
    quantity: number;
    type: string;
    item: string;
    material: string;
}

function App() {
    console.log('App rendered');
    const [detparts, setDetparts] = useState<DetailedPart[]>([]);
    const [tree, setTree] = useState<Node[]>([]);
    const [parentName, setParentName] = useState<string>('');
    const [currentPart, setCurrentPart] = useState<string>('');
    const [buttonDisabled, setButtonDisabled] = useState(false);
    useEffect(() => {
        console.log('useEffect called');
        populateParts();
    }, []);
    const handleClick = (id: number) => {
        console.log("clicked!")
        getSubGrid(id);
    };
    const handlePopClick = () => {
        populateTree();
        setButtonDisabled(true);
    }
    const dataGrid = detparts === undefined
        ? <p><em>Loading... Please refresh once the ASP.NET backend has started. See <a href="https://aka.ms/jspsintegrationreact">https://aka.ms/jspsintegrationreact</a> for more details.</em></p>
        : <div style={{ maxHeight: 'calc(100vh - 200px)' }}>
            <table className="table" aria-labelledby="tabelLabel">
                <thead>
                    <tr>
                        <th className='t-head'>PARENTNAME</th>
                        <th className='t-head'>COMPONENTNAME</th>
                        <th className='t-head'>PARTNUMBER</th>
                        <th className='t-head'>TITLE</th>
                        <th className='t-head'>QUANTITY</th>
                        <th className='t-head'>TYPE</th>
                        <th className='t-head'>ITEM</th>
                        <th className='t-head'>MATERIAL</th>
                    </tr>
                </thead>
                <tbody>
                    {detparts.map(part =>
                        <tr key={part.guid}>
                            <td>{part.parentName}</td>
                            <td>{part.componentName}</td>
                            <td>{part.partNumber}</td>
                            <td>{part.title}</td>
                            <td>{part.quantity}</td>
                            <td>{part.type}</td>
                            <td>{part.item}</td>
                            <td>{part.material}</td>
                        </tr>
                    )}
                </tbody>
            </table>
        </div>;

    return (
        <div className="container" style={{ height: '500px', backgroundColor: '#555' }}>
            <div className='row'>
                <div className="col" style={{ backgroundColor: '#333', borderBottom: '1px solid #959079' }}>
                    <h3 id="tabelLabel">Testing Functionality for Tree and Datagrid</h3>
                </div>
            </div>
            <div className='row' style={{ padding:'20px' }}>
                <div className="col">
                    <div style={{ width: '55%', height: '300px', overflow: 'scroll', backgroundColor: '#262626', color: 'white', overflowX: 'hidden' }}>
                        <TreeView data={tree} handleClick={handleClick} level={0} />
                    </div>
                </div>
                <div className="col" style={{ textAlign:'left' }}>
                    <p style={{ color: 'red' }}>Parent Child Part:  {parentName !== '' ? parentName + ' / ' : ''} {currentPart}</p>
                    <p style={{ color: 'lightgreen' }}><strong>Current Part: {currentPart}</strong> </p>
                    <p><button onClick={handlePopClick} className={buttonDisabled ? "disabled" : ""} style={{ color: 'white', backgroundColor: '#333', borderRadius: '3px' }} disabled={buttonDisabled}>Populate Data in Tree</button></p>
                </div>
            </div>
            <div className='row'>
                <div className="col" style={{ height: '30px', backgroundColor: '#333', borderTop: '1px solid #959079', borderBottom: '1px solid #959079' }}>
                </div>
            </div>
            <div className='row' >
                <div style={{ flex: '1', overflow: 'auto', maxHeight: '300px', padding: '20px', backgroundColor: 'rgb(85, 85, 85)' }}>
                    {dataGrid}
                </div>
            </div>
        </div>

    );

    
    function populateParts() {
        fetch('detparts')
            .then(response => response.json())
            .then(data => {
                try {
                    console.log('detparts data: ', data);
                   
                    setDetparts(data);
                } catch (e) {
                    console.error('Error parsing JSON:', e);
                }
            })
            .catch(error => {
                console.error('Error fetching parts:', error);
            });
    };

    function populateTree() {
        fetch('tree')
            .then(response => response.json())
            .then(data => {
                try {
                    console.log('poptree data: ', data);
                    
                    const newNodeArray: Node[] = [];
                    newNodeArray.push(data);
                    setTree(newNodeArray);
                } catch (error) {
                    console.error('Error parsing JSON:', error);
                }
            })
            .catch(error => {
                console.error('Error fetching tree:', error);
            });
    };
   
    function getSubGrid(id: number) {
        fetch(`subtree/${id}`)
            .then(response => response.json())
            .then(data => {
                try {
                    const partsArray: Node[] = [];
                    partsArray.push(data);

                    const flattenedPartsArray: DetailedPart[] = [];

                    function flattenNodes(nodes: Node[]) {
                      for (const node of nodes) {
                        const { children, name, ...rest } = node;
                        const flattenedPart = { ...rest, componentName: name };
                        flattenedPartsArray.push(flattenedPart);
                        flattenNodes(children);
                      }
                    }
                    flattenNodes(partsArray);
                    setParentName(flattenedPartsArray[0].parentName ?? '');
                    setCurrentPart(flattenedPartsArray[0].componentName);
                    if (flattenedPartsArray.length > 1) {
                        flattenedPartsArray.shift();
                    }
                    setDetparts(flattenedPartsArray);
                } catch (error) {
                    console.error('Error parsing JSON:', error);
                }
            })
            .catch(error => {
                console.error('Error fetching tree:', error);
            });
    };

}

export default App;