import { Node } from '../App';
import { useState } from 'react';


const TreeNode = ({ node, handleClick, level }: { node: Node, handleClick: any, level: number }) => {
    const [isOpen, setIsOpen] = useState(false);
    const toggle = (event: any) => {
        handleClick(node.id);
        event.stopPropagation();
        setIsOpen(!isOpen);
    };
    const paddingLeft = `${10 + level * 20}px`;

    return (
        <div className="tree-node" style={{ paddingLeft, paddingTop: '5px', textAlign: 'left' }}>
            {node.children && (
                <button onClick={toggle} className="toggle-icon"
                    style={{ background: 'none', border: 'none', color: '#d4d4d4' }}>
                    {isOpen ? 'v' : '>'}
                </button>)}
            <span>{node.name}</span>
            {isOpen && <TreeView data={node?.children} handleClick={handleClick} level={level + 1} />}
        </div>
    );
};

const TreeView = ({ data, handleClick, level }: { data: Node[], handleClick: any, level: number }) => {
    level = level || 0;
    return (
        <div className="tree-view" style={{ left: '0px', paddingLeft: '0px', paddingTop: '5px' }}>
            {Array.isArray(data) && data.map((node: Node, index: number) => (
                <TreeNode key={`${node.guid}-${index}`} node={node} handleClick={handleClick} level={level} />
            ))}
        </div>
    );
};

export default TreeView;